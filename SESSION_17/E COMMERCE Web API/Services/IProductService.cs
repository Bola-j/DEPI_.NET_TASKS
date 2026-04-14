using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Services
{
    public interface IProductService
    {
        Task<GenericResult<PagedResult<Product>>> GetAllAsync(string? search, int page, int pageSize);
        Task<GenericResult<Product>> GetByIdAsync(int id);
        Task<GenericResult<Product>> CreateAsync(Product product);
        Task<GenericResult<Product>> UpdateAsync(Product product);
        Task<GenericResult<Product>> DeleteAsync(int id);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<bool> ProductNameExistsAsync(string name, int? excludedId = null);
    }
}
