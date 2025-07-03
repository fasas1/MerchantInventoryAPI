using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Repository
{
    public class CustomerRepository :Repository<Customer>, ICustomerRepository
    {

        private readonly ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) :base(db) 
        {
            _db = db;
        }
        public async  Task<Customer> UpdateAsync(Customer entity)    
        {
             _db.Customers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
