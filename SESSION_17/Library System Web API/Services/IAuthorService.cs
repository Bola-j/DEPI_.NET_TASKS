using Library_System_Web_API.Entities;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Services
{
    public interface IAuthorService
    {
        Task<GenericResult<PagedResult<Author>>> GetAllAsync(string? name, int page, int pageSize);
        Task<GenericResult<Author>> GetByIdAsync(int id);
        Task<GenericResult<Author>> CreateAsync(Author author);
        Task<GenericResult<Author>> UpdateAsync(Author author);
        Task<GenericResult<Author>> DeleteAsync(int id);
    }
}
