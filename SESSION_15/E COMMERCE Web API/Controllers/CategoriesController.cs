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

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new SlimProductDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price
                    }).ToList()
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Products = c.Products.Select(p => new SlimProductDTO
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
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Category name is required.");
            }
            var category = new Category
            {
                Name = request.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            var response = new SlimCategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            };
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest request)
        {
            if (request == null)
            {
                return BadRequest("Category data is required.");
            }

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                if (!string.IsNullOrWhiteSpace(request.Name) && request.Name == "string")
                {
                    category = await _context.Categories.Where(c => c.Name == request.Name).FirstOrDefaultAsync();
                    if (category == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }


            if (!string.IsNullOrWhiteSpace(request.NewName) && request.NewName == "string")
            {
                category.Name = request.NewName;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category updated successfully",
                CategoryId = category.Id
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Category deleted successfully",
                CategoryId = id
            });
        }
    }
}