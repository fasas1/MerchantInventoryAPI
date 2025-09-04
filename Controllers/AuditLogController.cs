using MechantInventory.Model.Dto;
using MechantInventory.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MechantInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogRepo;

        public AuditLogController(IAuditLogRepository auditLogRepo)
        {
            _auditLogRepo = auditLogRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAllLogs()
        {
            var logs = await _auditLogRepo.GetAllAuditLogsAsync();
            return Ok(logs);
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetLogsByTransaction(int transactionId)
        {
            var logs = await _auditLogRepo.GetAuditLogsByTransactionAsync(transactionId);
            return Ok(logs);
        }
    }

}
