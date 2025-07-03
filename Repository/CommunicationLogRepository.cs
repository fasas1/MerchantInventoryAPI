using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Repository
{
    public class CommunicationLogRepository: Repository<CommunicationLog>, ICommunicationLogRepository
    {
        private readonly ApplicationDbContext _db;
        public CommunicationLogRepository(ApplicationDbContext db): base(db) 
        {
            _db = db;  
        }
        public async Task<CommunicationLog> UpdateAsync(CommunicationLog entity)
        {
            _db.CommunicationLogs.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
