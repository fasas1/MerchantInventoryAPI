
using MechantInventory.Model;

namespace MechantInventory.Repository.IRepository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> UpdateAsync(Customer entity);
    }
}
