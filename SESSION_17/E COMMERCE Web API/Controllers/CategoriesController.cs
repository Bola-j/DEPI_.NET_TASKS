using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using AutoMapper;
using E_COMMERCE_Web_API.Results;
using E_COMMERCE_Web_API.Services;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger, IMapper mapper)
        {
            _categoryService = categoryService;
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

            var result = await _categoryService.GetAllAsync(search, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<CategoryDTO>>.Failure(result.Error));
            }

            var categories = result.Value.Data.Select(_mapper.Map<CategoryDTO>).ToList();

            _logger.LogInformation($"Retrieved {categories.Count} categories (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");

            return Ok(GenericResult<PagedResult<CategoryDTO>>.Success(new PagedResult<CategoryDTO>(categories, page, pageSize, result.Value.TotalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<CategoryDTO>>> GetCategoryById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);


            if (result.IsFailure)
            {
                _logger.LogWarning($"Category with id {id} was not found.");
                return NotFound(GenericResult<CategoryDTO>.Failure($"Category with id {id} not found."));
            }

            _logger.LogInformation($"Retrieved category with id {id}.");
            return Ok(GenericResult<CategoryDTO>.Success(_mapper.Map<CategoryDTO>(result.Value)));
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
            var createResult = await _categoryService.CreateAsync(category);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimCategoryDTO>.Failure(createResult.Error));
            }
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

            var getResult = await _categoryService.GetByIdAsync(id);
            if (getResult.IsSuccess)
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

            var category = getResult.Value;

            _mapper.Map(request, category);


            var updateResult = await _categoryService.UpdateAsync(category);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimCategoryDTO>.Failure(updateResult.Error));
            }

            return Ok(GenericResult<SlimCategoryDTO>.Success(_mapper.Map<SlimCategoryDTO>(category)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimCategoryDTO>>> DeleteCategory(int id)
        {
            var deleteResult = await _categoryService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Category with id {id} was not found for deletion.");
                return NotFound(GenericResult<SlimCategoryDTO>.Failure($"Category with id {id} was not found."));
            }
            
            _logger.LogInformation($"Category with id {id} was deleted successfully.");

            return Ok(GenericResult<SlimCategoryDTO>.Success(_mapper.Map<SlimCategoryDTO>(deleteResult.Value)));
        }
    }
}