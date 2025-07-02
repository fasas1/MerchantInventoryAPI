using MechantInventory.Data;
using MechantInventory.Model.Dto;
using MechantInventory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace MechantInventory.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<InventoryReportDto>> GetInventoryReportAsync()
        {
            return await _db.Stocks
                .Include(x => x.Product)
            
                .Select(x => new InventoryReportDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductPrice = x.Product.Price,
                    Quantity = x.CurrentQuantity,
                    LowStock = x.Threshold < 5
                })
                .ToListAsync();
        }

        public async Task<List<SalesReportDto>> GetSalesReportAsync(DateTime start, DateTime end)
        {
            return await _db.Transactions
                .Include(s => s.Product)
                .Where(s => s.Timestamp >= start && s.Timestamp <= end)
                .GroupBy(s => s.Product.Name)
                .Select(g => new SalesReportDto
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(s => s.QuantityChanged),
                    TotalRevenue = g.Sum(s => s.QuantityChanged * s.Product.Price)
                })
                .ToListAsync();
        }

         
        public async Task<decimal> GetStockValuationAsync()
        {
            return await _db.Stocks
                .Include(s => s.Product)
                .SumAsync(s => s.CurrentQuantity * s.Product.Price);
        }

        public async Task<SummaryReportDto> GetSummaryReportAsync(DateTime start, DateTime end)
        {
            var sales = await _db.Transactions
                .Include(t => t.Product)
                .Where(t => t.TransactionType == "sale" && t.Timestamp >= start && t.Timestamp <= end)
                .ToListAsync();

            var restocks = await _db.Transactions
                .Where(t => t.TransactionType == "restock" && t.Timestamp >= start && t.Timestamp <= end)
                .ToListAsync();

            var totalRevenue = sales.Sum(s => s.QuantityChanged * s.Product.Price);
            var totalSalesCount = sales.Count;
            var totalRestockedItems = restocks.Sum(r => r.QuantityChanged);

            var topProduct = sales
                .GroupBy(s => s.Product.Name)
                .Select(g => new { ProductName = g.Key, TotalSold = g.Sum(s => s.QuantityChanged) })
                .OrderByDescending(g => g.TotalSold)
                .FirstOrDefault();

            return new SummaryReportDto
            {
                TotalRevenue = totalRevenue,
                TotalSalesCount = totalSalesCount,
                TotalRestockedItems = totalRestockedItems,
                TopSellingProduct = topProduct?.ProductName,
                TopSellingQuantity = topProduct?.TotalSold ?? 0
            };
        }

    }

}
