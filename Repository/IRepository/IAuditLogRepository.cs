using MechantInventory.Model;

namespace MechantInventory.Repository.IRepository
{
    public interface IAuditLogRepository :IRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetAuditLogsByTransactionAsync(int transactionId);
        Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync();
    }

}
