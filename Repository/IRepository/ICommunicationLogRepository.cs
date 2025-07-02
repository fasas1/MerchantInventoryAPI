
using MechantInventory.Model;

namespace MechantInventory.Repository.IRepository
{
    public interface ICommunicationLogRepository : IRepository<CommunicationLog>
    {
        Task<CommunicationLog> UpdateAsync(CommunicationLog entity);
    }
}
