using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GenericResult<PagedResult<Category>>> GetAllAsync(string? search, int page, int pageSize)
        {
            var categoriesData = await _categoryRepository.Query()
                .Include(c => c.Products)
                .ToListAsync();

            IEnumerable<Category> query = categoriesData;

            var searchTerm = search?.Trim();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var categories = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Category>>.Success(new PagedResult<Category>(categories, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Category>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.Query(false)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            return category is null
                ? GenericResult<Category>.Failure($"Category with id {id} not found.")
                : GenericResult<Category>.Success(category);
        }

        public Task<GenericResult<Category>> CreateAsync(Category category)
            => _categoryRepository.CreateAsync(category);

        public Task<GenericResult<Category>> UpdateAsync(Category category)
            => _categoryRepository.UpdateAsync(category);

        public Task<GenericResult<Category>> DeleteAsync(int id)
            => _categoryRepository.DeleteAsync(id);

        public async Task<bool> CategoryNameExistsAsync(string name, int? excludedId = null)
        {
            var categories = await _categoryRepository.Query().ToListAsync();
            return categories.Any(c => c.Name == name && (!excludedId.HasValue || c.Id != excludedId.Value));
        }
    }
}
