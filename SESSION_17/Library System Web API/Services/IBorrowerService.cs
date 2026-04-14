using Library_System_Web_API.Entities;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Services
{
    public interface IBorrowerService
    {
        Task<GenericResult<PagedResult<Borrower>>> GetAllAsync(int page, int pageSize);
        Task<GenericResult<Borrower>> GetByIdAsync(int id);
        Task<GenericResult<Borrower>> CreateAsync(Borrower borrower);
        Task<GenericResult<Borrower>> UpdateAsync(Borrower borrower);
        Task<GenericResult<Borrower>> DeleteAsync(int id);
    }
}
