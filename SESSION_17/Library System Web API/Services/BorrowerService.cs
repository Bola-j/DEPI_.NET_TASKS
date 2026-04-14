using Library_System_Web_API.Entities;
using Library_System_Web_API.Repositories;
using Library_System_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IGenericRepository<Borrower> _borrowerRepository;

        public BorrowerService(IGenericRepository<Borrower> borrowerRepository)
        {
            _borrowerRepository = borrowerRepository;
        }

        public async Task<GenericResult<PagedResult<Borrower>>> GetAllAsync(int page, int pageSize)
        {
            var borrowersData = await _borrowerRepository.Query()
                .Include(b => b.Loans)
                .ThenInclude(l => l.Book)
                .ThenInclude(b => b.Author)
                .ToListAsync();

            IEnumerable<Borrower> query = borrowersData;
            var totalCount = query.Count();

            var borrowers = query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Borrower>>.Success(new PagedResult<Borrower>(borrowers, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Borrower>> GetByIdAsync(int id)
        {
            var borrower = await _borrowerRepository.Query(false)
                .Include(b => b.Loans)
                    .ThenInclude(l => l.Book)
                        .ThenInclude(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);

            return borrower is null
                ? GenericResult<Borrower>.Failure($"Borrower with ID {id} not found.")
                : GenericResult<Borrower>.Success(borrower);
        }

        public Task<GenericResult<Borrower>> CreateAsync(Borrower borrower)
            => _borrowerRepository.CreateAsync(borrower);

        public Task<GenericResult<Borrower>> UpdateAsync(Borrower borrower)
            => _borrowerRepository.UpdateAsync(borrower);

        public Task<GenericResult<Borrower>> DeleteAsync(int id)
            => _borrowerRepository.DeleteAsync(id);
    }
}
