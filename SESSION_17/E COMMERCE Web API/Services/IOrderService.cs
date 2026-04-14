using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Services
{
    public interface IOrderService
    {
        Task<GenericResult<PagedResult<Order>>> GetAllAsync(string? search, int page, int pageSize);
        Task<GenericResult<Order>> GetByIdAsync(int id);
        Task<GenericResult<IEnumerable<Order>>> GetByCustomerAsync(int customerId);
        Task<GenericResult<Order>> CreateAsync(Order order);
        Task<GenericResult<Order>> UpdateAsync(Order order);
        Task<GenericResult<Order>> DeleteAsync(int id);
        Task<bool> CustomerExistsAsync(int customerId);
        Task<List<int>> GetMissingProductIdsAsync(IEnumerable<int> productIds);
    }
}
