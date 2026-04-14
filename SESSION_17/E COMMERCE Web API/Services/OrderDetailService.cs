using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IGenericRepository<OrderDetail> _orderDetailRepository;
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderDetailService(
            IGenericRepository<OrderDetail> orderDetailRepository,
            IGenericRepository<Order> orderRepository,
            IGenericRepository<Product> productRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<GenericResult<PagedResult<OrderDetail>>> GetAllAsync(int orderId, int page, int pageSize)
        {
            IQueryable<OrderDetail> query = _orderDetailRepository.Query()
                .Include(od => od.Order)
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId);

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(od => od.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return GenericResult<PagedResult<OrderDetail>>.Success(new PagedResult<OrderDetail>(data, page, pageSize, totalCount));
        }

        public async Task<GenericResult<OrderDetail>> GetByIdAsync(int orderId, int productId)
        {
            var detail = await _orderDetailRepository.Query()
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            return detail is null
                ? GenericResult<OrderDetail>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}.")
                : GenericResult<OrderDetail>.Success(detail);
        }

        public Task<GenericResult<OrderDetail>> CreateAsync(OrderDetail orderDetail)
            => _orderDetailRepository.CreateAsync(orderDetail);

        public Task<GenericResult<OrderDetail>> UpdateAsync(OrderDetail orderDetail)
            => _orderDetailRepository.UpdateAsync(orderDetail);

        public async Task<GenericResult<OrderDetail>> DeleteAsync(int orderId, int productId)
        {
            var detail = await _orderDetailRepository.Query(false)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail is null)
            {
                return GenericResult<OrderDetail>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}.");
            }

            detail.IsDeleted = true;
            return await _orderDetailRepository.UpdateAsync(detail);
        }

        public Task<bool> OrderExistsAsync(int orderId)
            => _orderRepository.Query().AnyAsync(o => o.Id == orderId);

        public Task<bool> ProductExistsAsync(int productId)
            => _productRepository.Query().AnyAsync(p => p.Id == productId);

        public Task<bool> ExistsAsync(int orderId, int productId)
            => _orderDetailRepository.Query().AnyAsync(od => od.OrderId == orderId && od.ProductId == productId);
    }
}
