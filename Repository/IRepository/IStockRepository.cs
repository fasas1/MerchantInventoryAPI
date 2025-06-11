using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Interface
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> UpdateAsync(Stock entity);
    }
}
