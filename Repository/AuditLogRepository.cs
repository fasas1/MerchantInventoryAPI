using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MechantInventory.Repository
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository   
    {
        private readonly ApplicationDbContext _db;

        public AuditLogRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public async Task<IEnumerable<AuditLog>> GetAuditLogsByTransactionAsync(int transactionId)
        {
            return await _db.AuditLogs
                .Where(l => l.TransactionId == transactionId)
                .OrderByDescending(l => l.Date)
                .ToListAsync();
        }
        public async Task<IEnumerable<AuditLog>> GetAllAuditLogsAsync()
        {
            return await _db.AuditLogs
                .OrderByDescending(l => l.Date)
                .ToListAsync();
        }
    }
}