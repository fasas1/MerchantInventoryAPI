using MerchantInventory.Models.Dto;
using System.Linq.Expressions;

namespace MechantInventory.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<PagedResult<T>> GetAllAsync(
         Expression<Func<T, bool>> filter = null,
         string? includeProperties = null,
         int pageSize = 10,
         int pageNumber = 1,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null
     );
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
