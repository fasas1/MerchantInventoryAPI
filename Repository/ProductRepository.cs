using MechantInventory.Data;
using MechantInventory.Model;
using MechantInventory.Interface;


namespace MechantInventory.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;
        }
        public async Task<Product> UpdateAsync(Product entity)
        {
            // entity.UpdatedDate = DateTime.Now;
            _db.Products.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }

      
    }
}
