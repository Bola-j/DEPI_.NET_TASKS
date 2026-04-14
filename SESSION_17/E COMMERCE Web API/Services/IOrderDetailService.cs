using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Services
{
    public interface IOrderDetailService
    {
        Task<GenericResult<PagedResult<OrderDetail>>> GetAllAsync(int orderId, int page, int pageSize);
        Task<GenericResult<OrderDetail>> GetByIdAsync(int orderId, int productId);
        Task<GenericResult<OrderDetail>> CreateAsync(OrderDetail orderDetail);
        Task<GenericResult<OrderDetail>> UpdateAsync(OrderDetail orderDetail);
        Task<GenericResult<OrderDetail>> DeleteAsync(int orderId, int productId);
        Task<bool> OrderExistsAsync(int orderId);
        Task<bool> ProductExistsAsync(int productId);
        Task<bool> ExistsAsync(int orderId, int productId);
    }
}
