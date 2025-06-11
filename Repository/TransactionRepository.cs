using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;
using Microsoft.EntityFrameworkCore;

namespace MechantInventory.Repository
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _db;
        public TransactionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Task<Transaction> UpdateAsync(Transaction entity)
        {
            throw new NotImplementedException();
        }
        public async Task<Transaction> GetTransactionWithDetailsAsync(int id)
        {
            return await _db.Transactions
                .Include(t => t.Product) // Assuming Product is a navigation property
                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }
    }
}
