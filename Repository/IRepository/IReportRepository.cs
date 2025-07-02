using MechantInventory.Model.Dto;

namespace MechantInventory.Repository.IRepository
{
    public interface IReportRepository
    {
        Task<List<InventoryReportDto>> GetInventoryReportAsync();
        Task<List<SalesReportDto>> GetSalesReportAsync(DateTime start, DateTime end);
        Task <SummaryReportDto> GetSummaryReportAsync(DateTime start, DateTime end);
        Task<decimal> GetStockValuationAsync();
    }
}
