using MechantInventory.Data;
using MechantInventory.Repository.IRepository;
using MerchantInventory.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MechantInventory.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<PagedResult<T>> GetAllAsync(
       Expression<Func<T, bool>> filter = null,
       string? includeProperties = null,
       int pageSize = 10,
       int pageNumber = 1,
       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
   )
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var totalCount = await query.CountAsync();
            if (pageSize > 0)
            {
                if (pageSize > 100) pageSize = 100;
                query = query
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize);
            }

             var items = await query.ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
             
            };
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // Include the specified navigation properties
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();

        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        //public Task Update(T entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
