using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product entity);
    }
}
