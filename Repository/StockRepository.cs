using MechantInventory.Data;
using MechantInventory.Interface;
using MechantInventory.Model;

namespace MechantInventory.Repository
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private readonly ApplicationDbContext _db;
        public StockRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Task<Stock> UpdateAsync(Stock entity)
        {
            throw new NotImplementedException();
        }
    }
}
