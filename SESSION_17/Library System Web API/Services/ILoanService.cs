using Library_System_Web_API.Entities;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Services
{
    public interface ILoanService
    {
        Task<GenericResult<PagedResult<Loan>>> GetAllAsync(string? name, int page, int pageSize);
        Task<GenericResult<PagedResult<Loan>>> GetDueLoansAsync(DateOnly date, int page, int pageSize);
        Task<GenericResult<Loan>> GetByBookAndBorrowerAsync(int bookId, int borrowerId);
        Task<GenericResult<Loan>> CreateAsync(Loan loan);
        Task<GenericResult<Loan>> UpdateAsync(Loan loan);
        Task<GenericResult<Loan>> DeleteByBookAndBorrowerAsync(int bookId, int borrowerId);
        Task<bool> BookExistsAsync(int bookId);
        Task<GenericResult<Borrower>> GetBorrowerByIdAsync(int borrowerId);
    }
}
