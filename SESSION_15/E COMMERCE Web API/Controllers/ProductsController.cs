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
        public async Task<IActionResult> CreateProduct(CreateProductReuestDTO productDto)
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

        //[HttpPut("api/products/{id}")]


    }

}