using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;

        public CustomerService(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<GenericResult<PagedResult<Customer>>> GetAllAsync(string? search, int page, int pageSize)
        {
            var query = _customerRepository.Query();

            var searchTerm = search?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{searchTerm}%"));
            }

            var totalCount = await query.CountAsync();
            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return GenericResult<PagedResult<Customer>>.Success(new PagedResult<Customer>(customers, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Customer>> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.Query()
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            return customer is null
                ? GenericResult<Customer>.Failure($"Customer with id {id} was not found.")
                : GenericResult<Customer>.Success(customer);
        }

        public Task<GenericResult<Customer>> CreateAsync(Customer customer)
            => _customerRepository.CreateAsync(customer);

        public Task<GenericResult<Customer>> UpdateAsync(Customer customer)
            => _customerRepository.UpdateAsync(customer);

        public Task<GenericResult<Customer>> DeleteAsync(int id)
            => _customerRepository.DeleteAsync(id);

        public Task<bool> EmailExistsAsync(string email, int? excludedId = null)
            => _customerRepository.Query().AnyAsync(c => c.Email == email && (!excludedId.HasValue || c.Id != excludedId.Value));
    }
}
