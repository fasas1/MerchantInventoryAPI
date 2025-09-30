using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MechantInventory.Repository
{
    public class ResellerRepository : Repository<Reseller>, IResellerRepository
    {
        private readonly ApplicationDbContext _db;
        public ResellerRepository(ApplicationDbContext db): base(db)
        {
            _db = db;  
        }
        public async Task<Reseller> GetResellerByIdAsync(int resellerId)
        {
            return await _db.Resellers
           .Include(r => r.Transactions)
               .ThenInclude(t => t.Items)
                   .ThenInclude(i => i.Product)
           .Include(r => r.Transactions)
               .ThenInclude(t => t.Payments)
           .Include(r => r.Transactions)
               .ThenInclude(t => t.Returns)
           .FirstOrDefaultAsync(r => r.ResellerId == resellerId); 

        }
        public async Task<ResellerTransaction> AddTransactionAsync(ResellerTransactionDto dto)
        {
            var reseller = await _db.Resellers.FindAsync(dto.ResellerId);
            if (reseller == null)
                throw new Exception("Reseller not found");

            decimal totalAmount = 0;
            var transaction = new ResellerTransaction
            {
                ResellerId = dto.ResellerId,
                Description = dto.Description,
                DueDate = dto.DueDate == default ? DateTime.UtcNow.AddDays(14) : dto.DueDate,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                Items = new List<ResellerTransactionItem>()
            };

            foreach (var itemDto in dto.Items)
            {
                var product = await _db.Products.Include(p => p.Stocks)
                                                .FirstOrDefaultAsync(p => p.ProductId == itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product {itemDto.ProductId} not found");

                var stock = product.Stocks.FirstOrDefault();
                if (stock == null || stock.CurrentQuantity < itemDto.Quantity)
                    throw new Exception($"Not enough stock for product {product.Name}");

                var item = new ResellerTransactionItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                transaction.Items.Add(item);

                totalAmount += item.Quantity * item.UnitPrice;

                // Decrease stock
                stock.CurrentQuantity -= itemDto.Quantity;
            }

            transaction.TotalAmount = totalAmount;

            //  Apply existing reseller credit
            if (reseller.CreditBalance > 0)
            {
                if (reseller.CreditBalance >= totalAmount)
                {
                    
                    transaction.AmountPaid = totalAmount;
                    transaction.Balance = 0;
                    transaction.Status = "Paid";

                    // Reduce credit
                    reseller.CreditBalance -= totalAmount;
                }
                else
                {
                    // Credit partially covers transaction
                    transaction.AmountPaid = reseller.CreditBalance;
                    transaction.Balance = totalAmount - reseller.CreditBalance;
                    transaction.Status = "Partial";

                    // Reset credit
                    reseller.CreditBalance = 0;
                }
            }
            else
            {
                transaction.AmountPaid = 0;
                transaction.Balance = totalAmount;
            }

            await _db.ResellerTransactions.AddAsync(transaction);
            await _db.SaveChangesAsync();

            return transaction;
        }


        public async Task<ResellerPayment> AddPaymentAsync(ResellerPaymentDto dto)
        {
            var transaction = await _db.ResellerTransactions
                .Include(t => t.Payments)
                .Include(t => t.Reseller)
                .FirstOrDefaultAsync(t => t.ResellerTransactionId == dto.ResellerTransactionId);

            if (transaction == null)
                throw new Exception("Transaction not found.");

            var payment = new ResellerPayment
            {
                ResellerTransactionId = dto.ResellerTransactionId,
                Amount = dto.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = dto.PaymentMethod,
                RecordedBy = dto.RecordedBy
            };

            transaction.Payments.Add(payment);

            // Update totals
            transaction.AmountPaid += dto.Amount;

            if (transaction.AmountPaid >= transaction.TotalAmount)
            {
                transaction.Balance = 0;
                transaction.Status = "Paid";
                transaction.OverPayment = transaction.AmountPaid - transaction.TotalAmount;

                if (transaction.OverPayment > 0)
                {
                  
                    transaction.Reseller.CreditBalance += transaction.OverPayment;

                    var audit = new AuditLog
                    {
                        TransactionId = transaction.ResellerTransactionId,
                        IssueType = "OverPayment",
                        Difference = transaction.OverPayment,
                        RecordedBy = dto.RecordedBy
                    };
                    await _db.AuditLogs.AddAsync(audit);
                }
            }
            else
            {
                transaction.Balance = transaction.TotalAmount - transaction.AmountPaid;
                transaction.Status = "Partial";
                transaction.OverPayment = 0;
            }

            await _db.SaveChangesAsync();
            return payment;
        }

        public async Task<ResellerReturn> AddReturnAsync(ResellerReturnDto dto)
        {
            var transaction = await _db.ResellerTransactions
                .Include(t => t.Reseller)
                .Include(t => t.Items)
                .Include(t => t.Returns)
                    .ThenInclude(r => r.Items)
                .FirstOrDefaultAsync(t => t.ResellerTransactionId == dto.ResellerTransactionId);

            if (transaction == null)
                throw new Exception("Transaction not found");

            var resellerReturn = new ResellerReturn
            {
                ResellerTransactionId = dto.ResellerTransactionId,
                Value = dto.Value,
                Reason = dto.Reason,
                ApprovedBy = dto.ApprovedBy,
                ReturnDate = DateTime.UtcNow
            };

            // Process each return item
            foreach (var rItemDto in dto.Items)
            {
                var trItem = transaction.Items
                    .FirstOrDefault(i => i.ResellerTransactionItemId == rItemDto.ResellerTransactionItemId);

                if (trItem == null)
                    throw new Exception("Transaction item not found");

                // Check if return > available qty
                if (rItemDto.QuantityReturned > (trItem.Quantity - trItem.ReturnedQuantity))
                    throw new Exception("Return exceeds purchased quantity");

                // Update returned qty
                trItem.ReturnedQuantity += rItemDto.QuantityReturned;

                // Create return item record
                var returnItem = new ResellerReturnItem
                {
                    ResellerTransactionItemId = trItem.ResellerTransactionItemId,
                    QuantityReturned = rItemDto.QuantityReturned,
                    UnitPrice = trItem.UnitPrice,
                    TotalPrice = trItem.UnitPrice * rItemDto.QuantityReturned
                };

                resellerReturn.Items.Add(returnItem);
            }

            // Update transaction totals
            transaction.TotalAmount -= resellerReturn.Value;
            if (transaction.TotalAmount < 0) transaction.TotalAmount = 0;

            if (transaction.AmountPaid > transaction.TotalAmount)
            {
                var excess = transaction.AmountPaid - transaction.TotalAmount;
                transaction.OverPayment = excess;
                transaction.Balance = 0;
                transaction.Reseller.CreditBalance += excess;
            }
            else
            {
                transaction.Balance = transaction.TotalAmount - transaction.AmountPaid;
                transaction.OverPayment = 0;
            }

            transaction.Status = transaction.Balance <= 0 ? "Paid" : "Partial";

            transaction.Returns.Add(resellerReturn);
            await _db.SaveChangesAsync();

            return resellerReturn;
        }


    }
}

        