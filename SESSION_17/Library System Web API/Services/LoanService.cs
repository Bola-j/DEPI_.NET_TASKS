using Library_System_Web_API.Entities;
using Library_System_Web_API.Repositories;
using Library_System_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Services
{
    public class LoanService : ILoanService
    {
        private readonly IGenericRepository<Loan> _loanRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Borrower> _borrowerRepository;

        public LoanService(
            IGenericRepository<Loan> loanRepository,
            IGenericRepository<Book> bookRepository,
            IGenericRepository<Borrower> borrowerRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _borrowerRepository = borrowerRepository;
        }

        public async Task<GenericResult<PagedResult<Loan>>> GetAllAsync(string? name, int page, int pageSize)
        {
            var loansData = await _loanRepository.Query()
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .ToListAsync();

            IEnumerable<Loan> query = loansData;
            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(l =>
                    (l.Borrower != null && l.Borrower.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)) ||
                    (l.Book != null && l.Book.Title.Contains(searchName, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = query.Count();
            var loans = query
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Loan>>.Success(new PagedResult<Loan>(loans, page, pageSize, totalCount));
        }

        public async Task<GenericResult<PagedResult<Loan>>> GetDueLoansAsync(DateOnly date, int page, int pageSize)
        {
            var loansData = await _loanRepository.Query()
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .ToListAsync();

            IEnumerable<Loan> query = loansData.Where(l => l.ReturnDate == date);

            var totalCount = query.Count();
            var loans = query
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Loan>>.Success(new PagedResult<Loan>(loans, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Loan>> GetByBookAndBorrowerAsync(int bookId, int borrowerId)
        {
            var loan = await _loanRepository.Query(false)
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId);

            return loan is null
                ? GenericResult<Loan>.Failure($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}.")
                : GenericResult<Loan>.Success(loan);
        }

        public Task<GenericResult<Loan>> CreateAsync(Loan loan)
            => _loanRepository.CreateAsync(loan);

        public Task<GenericResult<Loan>> UpdateAsync(Loan loan)
            => _loanRepository.UpdateAsync(loan);

        public async Task<GenericResult<Loan>> DeleteByBookAndBorrowerAsync(int bookId, int borrowerId)
        {
            var loan = await _loanRepository.Query(false)
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId);

            if (loan is null)
            {
                return GenericResult<Loan>.Failure($"Loan with BookId {bookId} and BorrowerId {borrowerId} not found.");
            }

            return await _loanRepository.DeleteAsync(loan.Id);
        }

        public async Task<bool> BookExistsAsync(int bookId)
        {
            return await _bookRepository.Query().AnyAsync(b => b.Id == bookId);
        }

        public async Task<GenericResult<Borrower>> GetBorrowerByIdAsync(int borrowerId)
        {
            var borrower = await _borrowerRepository.Query(false)
                .FirstOrDefaultAsync(b => b.Id == borrowerId);

            return borrower is null
                ? GenericResult<Borrower>.Failure($"Invalid BorrowerId: {borrowerId}.")
                : GenericResult<Borrower>.Success(borrower);
        }
    }
}
