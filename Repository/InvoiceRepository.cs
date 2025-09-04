using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Repository.IRepository;

namespace MechantInventory.Repository
{
    public class InvoiceRepository :Repository<Invoice>, IInvoiceRepository
    {

        private readonly ApplicationDbContext _db;
        public InvoiceRepository(ApplicationDbContext db) :base(db) 
        {
            _db = db;
        }
        public async  Task<Invoice> UpdateAsync(Invoice entity)    
        {
             _db.Invoices.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
