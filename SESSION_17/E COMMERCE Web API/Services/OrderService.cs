using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepository;
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Product> _productRepository;

        public OrderService(
            IGenericRepository<Order> orderRepository,
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Product> productRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
        }

        public async Task<GenericResult<PagedResult<Order>>> GetAllAsync(string? search, int page, int pageSize)
        {
            IQueryable<Order> query = _orderRepository.Query()
                .Include(o => o.Customer);

            var searchTerm = search?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o => o.Customer != null && EF.Functions.Like(o.Customer.Name, $"%{searchTerm}%"));
            }

            var totalCount = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return GenericResult<PagedResult<Order>>.Success(new PagedResult<Order>(orders, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Order>> GetByIdAsync(int id)
        {
            var order = await _orderRepository.Query()
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order is null
                ? GenericResult<Order>.Failure($"Order with id {id} was not found.")
                : GenericResult<Order>.Success(order);
        }

        public async Task<GenericResult<IEnumerable<Order>>> GetByCustomerAsync(int customerId)
        {
            var orders = await _orderRepository.Query()
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();

            return GenericResult<IEnumerable<Order>>.Success(orders);
        }

        public Task<GenericResult<Order>> CreateAsync(Order order)
            => _orderRepository.CreateAsync(order);

        public Task<GenericResult<Order>> UpdateAsync(Order order)
            => _orderRepository.UpdateAsync(order);

        public Task<GenericResult<Order>> DeleteAsync(int id)
            => _orderRepository.DeleteAsync(id);

        public Task<bool> CustomerExistsAsync(int customerId)
            => _customerRepository.Query().AnyAsync(c => c.Id == customerId);

        public async Task<List<int>> GetMissingProductIdsAsync(IEnumerable<int> productIds)
        {
            var ids = productIds.Distinct().ToList();
            var existingIds = await _productRepository.Query()
                .Where(p => ids.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            return ids.Except(existingIds).ToList();
        }
    }
}
