using AutoMapper;
using Azure.Core;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;
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
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ECommerceDbContext context, IMapper mapper, ILogger<ProductsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<ProductDTO>>>> GetAllProducts(string? search, int page = 1, int pageSize = 10)
        {
            if(page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}");
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var query = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();
            var searchTerm = search?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm)) 
            { 
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchTerm}%"));
            }

            var totalCount = await query.CountAsync();
            
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize) 
                .Select(p => _mapper.Map<ProductDTO>(p))
                .ToListAsync();
            _logger.LogInformation($"Retrieved {products.Count} products (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");
            return Ok(GenericResult<PagedResult<ProductDTO>>.Success(new PagedResult<ProductDTO>(products, page, pageSize, totalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<ProductDTO>>> GetProductById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} was not found."); 
                return NotFound(GenericResult<ProductDTO>.Failure("Product not found."));
            }
            _logger.LogInformation($"Product with id {id} was retrieved successfully.");
            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(product)));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<ProductDTO>>> CreateProduct(CreateProductRequestDTO productDto)
        {
            if (productDto != null)
            {
                if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Name == "" || productDto.Name == "string")
                {
                    _logger.LogWarning("Invalid product name provided.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("Invalid product name."));
                }
                if(productDto.Price <= 0)
                {
                    _logger.LogWarning("Invalid product price provided.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("Price must be greater than zero."));
                }
                
                 var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == productDto.CategoryId);
                if(!categoryExists) {
                    _logger.LogWarning($"Category with id {productDto.CategoryId} does not exist.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("Invalid category."));
                }


                var product = _mapper.Map<Product>(productDto);

                _logger.LogInformation($"Creating product with name: {product.Name}, price: {product.Price}, categoryId: {product.CategoryId}.");
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Product with id {product.Id} was created successfully in the database.");

                _logger.LogInformation($"Product with id {product.Id} was created successfully.");
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(product)));   
            }

            return BadRequest("Invalid product data.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<ProductDTO>>> UpdateProduct(int id, UpdateProductRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Update product request is null.");
                return BadRequest(GenericResult<ProductDTO>.Failure("Product data is required."));
            }

            var product = await _context.Products
                .FindAsync(id);

            if (product == null)  
            {
                _logger.LogWarning($"Product with id {id} was not found for update.");
                return NotFound(GenericResult<ProductDTO>.Failure($"Product with Id = {id} not found."));
            }

            if (!string.IsNullOrWhiteSpace(request.NewProductName))
            {
                bool exists = await _context.Products
                    .AnyAsync(p => p.Name == request.NewProductName && p.Id != id);

                if (exists)
                {

                    _logger.LogWarning($"A product with the name {request.NewProductName} already exists.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("A product with this name already exists."));
                }

                //product.Name = request.NewProductName;
            }

            if (request.NewCategoryId.HasValue && request.NewCategoryId > 0)
            {
                var category = await _context.Categories.FindAsync(request.NewCategoryId.Value);

                if (category == null)
                {

                    _logger.LogWarning($"Category with id {request.NewCategoryId.Value} was not found.");
                    return NotFound(GenericResult<ProductDTO>.Failure("Category not found."));
                }

                //product.CategoryId = category.Id;

            }


            if (!request.ProductPrice.HasValue || request.ProductPrice < 0)
            {
                _logger.LogWarning($"Invalid product price: {request.ProductPrice}");
                return BadRequest(GenericResult<ProductDTO>.Failure("Invalid product price."));
            }

            _mapper.Map(request, product);
            _logger.LogInformation($"Updating product with id {id} with new values: Name={request.NewProductName}, Price={request.ProductPrice}, CategoryId={request.NewCategoryId}.");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product with id {id} was updated successfully in the database.");
            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(product)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<ProductDTO>>> DeleteProduct(int id)
        {
            var product = await _context.Products
                .FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} was not found for deletion.");
                return NotFound(GenericResult<ProductDTO>.Failure($"Product with Id = {id} not found."));
            }


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Product with id {id} was deleted successfully from the database.");

            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(product)));
        }
    }
}