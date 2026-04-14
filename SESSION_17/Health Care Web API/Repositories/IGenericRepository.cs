using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;

namespace Health_Care_Web_API.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query(bool asNoTracking = true);
        Task<GenericResult<PagedResult<T>>> GetAllAsync(string? search, int pageNumber, int pageSize);
        Task<GenericResult<T>> GetByIdAsync(int id);
        Task<GenericResult<T>> CreateAsync(T entity);
        Task<GenericResult<T>> UpdateAsync(T entity);
        Task<GenericResult<T>> DeleteAsync(int id);
    }
}
