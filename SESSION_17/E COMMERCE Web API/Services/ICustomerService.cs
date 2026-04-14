using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Services
{
    public interface ICustomerService
    {
        Task<GenericResult<PagedResult<Customer>>> GetAllAsync(string? search, int page, int pageSize);
        Task<GenericResult<Customer>> GetByIdAsync(int id);
        Task<GenericResult<Customer>> CreateAsync(Customer customer);
        Task<GenericResult<Customer>> UpdateAsync(Customer customer);
        Task<GenericResult<Customer>> DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludedId = null);
    }
}
