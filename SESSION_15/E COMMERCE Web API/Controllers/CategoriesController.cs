using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public CategoriesController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new SlimProductsResponseDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("api/categories/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new SlimProductsResponseDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

    }
}
