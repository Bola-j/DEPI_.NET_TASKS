using Library_System_Web_API.Entities;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Services
{
    public interface IBookService
    {
        Task<GenericResult<PagedResult<Book>>> GetAllAsync(string? name, int page, int pageSize);
        Task<GenericResult<Book>> GetByIdAsync(int id);
        Task<GenericResult<Book>> CreateAsync(Book book);
        Task<GenericResult<Book>> UpdateAsync(Book book);
        Task<GenericResult<Book>> DeleteAsync(int id);
        Task<bool> AuthorExistsAsync(int authorId);
    }
}
