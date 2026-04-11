using Azure.Core;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            // I am using CategoryResponseDTO to only show Id , Name of the category
            var products = await _context.Products
                .AsNoTracking()
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category != null ? new SlimCategoryDTO
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    } : null
                })
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.Category != null ? new SlimCategoryDTO
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductRequestDTO productDto)
        {
            if (productDto != null)
            {
                if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Name == "" || productDto.Name == "string")
                {
                    return BadRequest("Invalid product name.");
                }
                if(productDto.Price <= 0)
                {
                    return BadRequest("Price must be greater than zero.");
                }
                
                 var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == productDto.CategoryId);
                if(!categoryExists) {
                    return BadRequest("Invalid category.");
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
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

            if (request.NewCategoryId.HasValue && request.NewCategoryId > 0)
            {
                var category = await _context.Categories.FindAsync(request.NewCategoryId.Value);

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound($"Product with Id = {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Product deleted successfully",
                ProductId = product.Id
            });
        }
    }
}