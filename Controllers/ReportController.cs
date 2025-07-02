using MechantInventory.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MechantInventory.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;

        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventoryReport()
        {
            var result = await _reportRepository.GetInventoryReportAsync();
            return Ok(result);
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await _reportRepository.GetSalesReportAsync(start, end);
            return Ok(result);
        }

        [HttpGet("stock-valuation")]
        public async Task<IActionResult> GetStockValuation()
        {
            var value = await _reportRepository.GetStockValuationAsync();
            return Ok(new { TotalValue = value });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaryReport([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var summary = await _reportRepository.GetSummaryReportAsync(start, end);
            return Ok(summary);
        }

    }

}
