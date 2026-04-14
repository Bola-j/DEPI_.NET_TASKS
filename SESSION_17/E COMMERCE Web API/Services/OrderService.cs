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
            var loadedOrders = await _orderRepository.Query()
                .Include(o => o.Customer)
                .ToListAsync();
            IEnumerable<Order> query = loadedOrders;

            var searchTerm = search?.Trim();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(o => o.Customer != null && o.Customer.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var orders = query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Order>>.Success(new PagedResult<Order>(orders, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Order>> GetByIdAsync(int id)
        {
            var order = await _orderRepository.Query(false)
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
            var ordersData = await _orderRepository.Query().ToListAsync();
            var orders = ordersData.Where(o => o.CustomerId == customerId).ToList();

            return GenericResult<IEnumerable<Order>>.Success(orders);
        }

        public Task<GenericResult<Order>> CreateAsync(Order order)
            => _orderRepository.CreateAsync(order);

        public Task<GenericResult<Order>> UpdateAsync(Order order)
            => _orderRepository.UpdateAsync(order);

        public Task<GenericResult<Order>> DeleteAsync(int id)
            => _orderRepository.DeleteAsync(id);

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            var customers = await _customerRepository.Query().ToListAsync();
            return customers.Any(c => c.Id == customerId);
        }

        public async Task<List<int>> GetMissingProductIdsAsync(IEnumerable<int> productIds)
        {
            var ids = productIds.Distinct().ToList();
            var products = await _productRepository.Query().ToListAsync();
            var existingIds = products
                .Where(p => ids.Contains(p.Id))
                .Select(p => p.Id)
                .ToList();

            return ids.Except(existingIds).ToList();
        }
    }
}
