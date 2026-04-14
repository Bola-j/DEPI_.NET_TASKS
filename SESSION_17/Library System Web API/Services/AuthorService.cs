using Library_System_Web_API.Entities;
using Library_System_Web_API.Repositories;
using Library_System_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;

        public AuthorService(IGenericRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<GenericResult<PagedResult<Author>>> GetAllAsync(string? name, int page, int pageSize)
        {
            var authorsData = await _authorRepository.Query()
                .Include(a => a.Books)
                .ToListAsync();

            IEnumerable<Author> query = authorsData;
            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(a => a.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var authors = query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Author>>.Success(new PagedResult<Author>(authors, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Author>> GetByIdAsync(int id)
        {
            var author = await _authorRepository.Query(false)
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            return author is null
                ? GenericResult<Author>.Failure($"Author with ID {id} not found.")
                : GenericResult<Author>.Success(author);
        }

        public Task<GenericResult<Author>> CreateAsync(Author author)
            => _authorRepository.CreateAsync(author);

        public Task<GenericResult<Author>> UpdateAsync(Author author)
            => _authorRepository.UpdateAsync(author);

        public Task<GenericResult<Author>> DeleteAsync(int id)
            => _authorRepository.DeleteAsync(id);
    }
}
