using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Services
{
    public interface ICategoryService
    {
        Task<GenericResult<PagedResult<Category>>> GetAllAsync(string? search, int page, int pageSize);
        Task<GenericResult<Category>> GetByIdAsync(int id);
        Task<GenericResult<Category>> CreateAsync(Category category);
        Task<GenericResult<Category>> UpdateAsync(Category category);
        Task<GenericResult<Category>> DeleteAsync(int id);
        Task<bool> CategoryNameExistsAsync(string name, int? excludedId = null);
    }
}
