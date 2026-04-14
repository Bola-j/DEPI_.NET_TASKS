using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Category> _categoryRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<GenericResult<PagedResult<Product>>> GetAllAsync(string? search, int page, int pageSize)
        {
            var productsData = await _productRepository.Query()
                .Include(p => p.Category)
                .ToListAsync();

            IEnumerable<Product> query = productsData;

            var searchTerm = search?.Trim();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();
            var products = query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return GenericResult<PagedResult<Product>>.Success(new PagedResult<Product>(products, page, pageSize, totalCount));
        }

        public async Task<GenericResult<Product>> GetByIdAsync(int id)
        {
            var product = await _productRepository.Query(false)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product is null
                ? GenericResult<Product>.Failure("Product not found.")
                : GenericResult<Product>.Success(product);
        }

        public Task<GenericResult<Product>> CreateAsync(Product product)
            => _productRepository.CreateAsync(product);

        public Task<GenericResult<Product>> UpdateAsync(Product product)
            => _productRepository.UpdateAsync(product);

        public Task<GenericResult<Product>> DeleteAsync(int id)
            => _productRepository.DeleteAsync(id);

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            var categories = await _categoryRepository.Query().ToListAsync();
            return categories.Any(c => c.Id == categoryId);
        }

        public async Task<bool> ProductNameExistsAsync(string name, int? excludedId = null)
        {
            var products = await _productRepository.Query().ToListAsync();
            return products.Any(p => p.Name == name && (!excludedId.HasValue || p.Id != excludedId.Value));
        }
    }
}
