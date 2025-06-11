using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Interface
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction> UpdateAsync(Transaction entity);
        Task<Transaction> GetTransactionWithDetailsAsync(int id);
    }
}
