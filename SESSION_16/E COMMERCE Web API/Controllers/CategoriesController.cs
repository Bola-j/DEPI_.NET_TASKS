using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using AutoMapper;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ECommerceDbContext context, ILogger<CategoriesController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<CategoryDTO>>>> GetAllCategories(string? search = null, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<CategoryDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Categories
                .Include(c => c.Products)
                .Select(c => _mapper.Map<CategoryDTO>(c))
                .AsNoTracking();

            var searchTerm = search?.Trim().ToLower(); 
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{searchTerm}%"));
            

            var totalCount = await query.CountAsync();

            var categories = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation($"Retrieved {categories.Count} categories (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");

            return Ok(GenericResult<PagedResult<CategoryDTO>>.Success(new PagedResult<CategoryDTO>(categories, page, pageSize, totalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<CategoryDTO>>> GetCategoryById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.Id == id)
                .Select(c => _mapper.Map<CategoryDTO>(c))
                .FirstOrDefaultAsync();


            if (category == null)
            {
                _logger.LogWarning($"Category with id {id} was not found.");
                return NotFound(GenericResult<CategoryDTO>.Failure($"Category with id {id} not found."));
            }

            _logger.LogInformation($"Retrieved category with id {id}.");
            return Ok(GenericResult<CategoryDTO>.Success(category));
        }
        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimCategoryDTO>>> CreateCategory(CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
                _logger.LogWarning("Invalid category name.");
                return BadRequest(GenericResult<SlimCategoryDTO>.Failure("Invalid category name."));
            }
            
            var category = _mapper.Map<Category>(request);
            
            _logger.LogInformation($"Creating new category with name: {category.Name}.");
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Category created with id: {category.Id}.");

            //var response = new SlimCategoryDTO
            //{
            //    Id = category.Id,
            //    Name = category.Name
            //};
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, GenericResult<SlimCategoryDTO>.Success(_mapper.Map<SlimCategoryDTO>(category)));
        }

        [HttpPut]
        public async Task<ActionResult<GenericResult<SlimCategoryDTO>>> UpdateCategory(int id, UpdateCategoryRequest request)
        {

            if (request == null)
            {
                return BadRequest(GenericResult<SlimCategoryDTO>.Failure("Category data is required."));
            }

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                if (string.IsNullOrWhiteSpace(request.NewName) || request.NewName == "" || request.NewName == "string")
                {
                    _logger.LogWarning($"Invalid category name provided for update of category with id {id}.");
                    return BadRequest(GenericResult<SlimCategoryDTO>.Failure("Invalid category name."));
                }
                
            }
            else 
            {   _logger.LogWarning($"Category with id {id} was not found for update.");
                return NotFound(GenericResult<SlimCategoryDTO>.Failure($"Category with id {id} was not found."));
            }

            _mapper.Map(request, category);


            await _context.SaveChangesAsync();

            return Ok(GenericResult<SlimCategoryDTO>.Success(_mapper.Map<SlimCategoryDTO>(category)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimCategoryDTO>>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category with id {id} was not found for deletion.");
                return NotFound(GenericResult<SlimCategoryDTO>.Failure($"Category with id {id} was not found."));
            }
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Category with id {id} was deleted successfully.");

            return Ok(GenericResult<SlimCategoryDTO>.Success(_mapper.Map<SlimCategoryDTO>(category)));
        }
    }
}