using Library_System_Web_API.Entities;
using Library_System_Web_API.Repositories;
using Library_System_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Author> _authorRepository;

        public BookService(IGenericRepository<Book> bookRepository, IGenericRepository<Author> authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<GenericResult<PagedResult<Book>>> GetAllAsync(string? name, int page, int pageSize)
        {
            var booksData = await _bookRepository.Query()
                .Include(b => b.Author)
                .Include(b => b.Loans)
                .ThenInclude(l => l.Borrower)
                .ToListAsync();

            IEnumerable<Book> query = booksData;
            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchName, StringComparison.OrdinalIgnoreCase) ||
                    (b.Author != null && b.Author.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)));
            }

            var totalCount = query.Count();
            var books = query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Book>>.Success(new PagedResult<Book>(books, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Book>> GetByIdAsync(int id)
        {
            var book = await _bookRepository.Query(false)
                .Include(b => b.Author)
                .Include(b => b.Loans)
                    .ThenInclude(l => l.Borrower)
                .FirstOrDefaultAsync(b => b.Id == id);

            return book is null
                ? GenericResult<Book>.Failure($"Book with ID {id} not found.")
                : GenericResult<Book>.Success(book);
        }

        public Task<GenericResult<Book>> CreateAsync(Book book)
            => _bookRepository.CreateAsync(book);

        public Task<GenericResult<Book>> UpdateAsync(Book book)
            => _bookRepository.UpdateAsync(book);

        public Task<GenericResult<Book>> DeleteAsync(int id)
            => _bookRepository.DeleteAsync(id);

        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            return await _authorRepository.Query().AnyAsync(a => a.Id == authorId);
        }
    }
}
