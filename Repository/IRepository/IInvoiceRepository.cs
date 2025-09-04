
using MechantInventory.Model;

namespace MechantInventory.Repository.IRepository
{
    public interface IInvoiceRepository : IRepository<Invoice>
    {
        Task<Invoice> UpdateAsync(Invoice entity);
    }
}
