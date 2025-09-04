using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Model.Dto;
using MechantInventory.Repository;
using MechantInventory.Repository.IRepository;
using MechantInventory.Utility;
using MerchantInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Net;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private ApiResponse _response;
        private readonly ApplicationDbContext _db;
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceController(ApplicationDbContext db, IInvoiceRepository invoiceRepository)
        {
            _db = db;
            _invoiceRepository = invoiceRepository;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetInvoices(
        [FromQuery] Guid? customerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            Expression<Func<Invoice, bool>> filter = null;

            if (customerId.HasValue && customerId != Guid.Empty)
            {
                filter = i => i.CustomerId == customerId.Value;
            }

            var invoiceList = await _invoiceRepository.GetAllAsync(
                filter: filter,
                includeProperties: "Customer,InvoiceItems.Product",
                pageSize: pageSize,
                pageNumber: pageNumber
            );

            _response.IsSuccess = true;
            _response.Result = invoiceList;
            _response.StatusCode = HttpStatusCode.OK;

            return Ok(_response);
        }


        [HttpGet("{id:guid}", Name = "GetInvoice")]
        public async Task<ActionResult<ApiResponse>> GetCustomer(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                Invoice invoice = await _invoiceRepository.GetAsync(u => u.InvoiceId == id);
                if (invoice == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.Result = invoice;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Staff)]
        public async Task<ActionResult<ApiResponse>> CreateInvoice([FromBody] CreateInvoiceDto invoiceDto)
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"CLAIM TYPE: {claim.Type}   VALUE: {claim.Value}");
            }

            try
            {
                if (invoiceDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var customerExists = await _db.Customers
                    .AnyAsync(c => c.CustomerId == invoiceDto.CustomerId);

                if (!customerExists)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string> { "Customer does not exist." };
                    return BadRequest(_response);
                }

                var invoiceId = Guid.NewGuid();
                var invoiceNumber = $"INV-{DateTime.UtcNow.Ticks}";

                var invoiceItems = new List<InvoiceItem>();
                var transactions = new List<Transaction>();

                foreach (var item in invoiceDto.InvoiceItems)
                {
                    // Check stock
                    var stock = await _db.Stocks
                        .Include(s => s.Product)
                        .FirstOrDefaultAsync(s => s.ProductId == item.ProductId);

                    if (stock == null)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string> { $"Product ID {item.ProductId} does not exist in stock." };
                        return BadRequest(_response);
                    }

                    if (stock.CurrentQuantity < item.Quantity)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string> { $"Not enough stock for {stock.Product.Name}. Available: {stock.CurrentQuantity}, Requested: {item.Quantity}" };
                        return BadRequest(_response);
                    }

                    // Reduce stock
                    stock.CurrentQuantity -= item.Quantity;

                    // Create invoice item
                    invoiceItems.Add(new InvoiceItem
                    {
                        InvoiceItemId = Guid.NewGuid(),
                        InvoiceId = invoiceId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                    var currentUser =
                  User?.Claims.FirstOrDefault(c => c.Type == "fullName")?.Value
                  ?? User?.Claims.FirstOrDefault(c => c.Type == "email")?.Value
                  ?? "System";


                    // Create transaction
                    transactions.Add(new Transaction
                    {
                        ProductId = item.ProductId,
                        QuantityChanged = item.Quantity,
                        TransactionType = "Sale",
                        Timestamp = DateTime.UtcNow,
                        Description = $"Sold via invoice {invoiceNumber}",
                        PerformedBy = currentUser
                    });
                }

                var totalAmount = invoiceItems.Sum(i => i.Quantity * i.UnitPrice);

                var invoice = new Invoice
                {
                    InvoiceId = invoiceId,
                    InvoiceNumber = invoiceNumber,
                    CustomerId = invoiceDto.CustomerId,
                    InvoiceDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    PaymentStatus = invoiceDto.PaymentStatus,
                    InvoiceItems = invoiceItems
                };

                await _invoiceRepository.CreateAsync(invoice);

                // Save all new transactions
                await _db.Transactions.AddRangeAsync(transactions);

                await _db.SaveChangesAsync();

             

                _response.IsSuccess = true;
                _response.Result = invoice;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute(
                    "GetInvoice",
                    new { id = invoice.InvoiceId },
                    _response
                );
            }
            catch (Exception ex)
            {
               

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.InnerException?.Message ?? ex.Message };
                return StatusCode(500, _response);
            }
        }



    }
}
