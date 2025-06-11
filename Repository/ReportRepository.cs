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
             
                    Quantity = x.CurrentQuantity,
                    LowStock = x.Threshold < 10
                })
                .ToListAsync();
        }

        public async Task<List<SalesReportDto>> GetSalesReportAsync(DateTime start, DateTime end)
        {
            return await _db.Transactions
                .Include(s => s.Product)
                .Where(s => s.Timestamp  >= start && s.Timestamp <= end)
                .GroupBy(s => s.Product.Name)
                .Select(g => new SalesReportDto
                {
                    ProductName = g.Key,
                    TotalQuantitySold = g.Sum(s => s.QuantityChanged),
                    TotalRevenue = g.Sum(s => s.QuantityChanged * s.QuantityChanged)
                })
                .ToListAsync();
        }

        public async Task<decimal> GetStockValuationAsync()
        {
            return await _db.Stocks
                .Include(s => s.Product)
                .SumAsync(s => s.CurrentQuantity * s.Product.Price);
        }
    }

}
