using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Entities;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public ProductsController(ECommerceDbContext context)
        {
            _context = context;
        }


        [HttpGet("api/products")]
        public async Task<IActionResult> GetAllProducts()
        {
            // I am using CategoryResponseDTO to only show Id , Name of the category
            var products = await _context.Products
                .AsNoTracking()
                .Select(p => new ProductsResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category != null ? new SlimCategoryResponseDTO
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    } : null
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("api/products/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductsResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category != null ? new SlimCategoryResponseDTO
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    } : null
                })
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost("api/products")]
        public async Task<IActionResult> CreateProduct(CreateProductRequestDTO productDto)
        {
            if (productDto != null)
            {
                if (string.IsNullOrWhiteSpace(productDto.Name))
                {
                    return BadRequest("Product name is required.");
                }
                var product = new Product
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryId = productDto.CategoryId
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return Created();
            }

            return BadRequest("Invalid product data.");
        }

        [HttpPut("api/products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequestDTO request)
        {
            if (request == null)
                return BadRequest("Product data is required.");

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            if (!string.IsNullOrWhiteSpace(request.NewProductName))
            {
                bool exists = await _context.Products
                    .AnyAsync(p => p.Name == request.NewProductName && p.Id != id);

                if (exists)
                    return BadRequest("A product with this name already exists.");

                product.Name = request.NewProductName;
            }

            if (request.NewCategoryId.HasValue)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.NewCategoryId.Value);

                if (category == null)
                    return NotFound("Category not found.");

                product.CategoryId = category.Id;
            }
            else if (!string.IsNullOrWhiteSpace(request.NewCategoryName))
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == request.NewCategoryName);

                if (category == null)
                    return NotFound("Category not found.");

                product.CategoryId = category.Id;
            }

            if (request.ProductPrice.HasValue && request.ProductPrice > 0)
            {
                product.Price = request.ProductPrice.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Product updated successfully",
                ProductId = product.Id
            });
        }
    }
}