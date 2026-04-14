using AutoMapper;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;
using E_COMMERCE_Web_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productService = productService;
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

            var result = await _productService.GetAllAsync(search, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<ProductDTO>>.Failure(result.Error));
            }

            var products = result.Value.Data.Select(_mapper.Map<ProductDTO>).ToList();
            _logger.LogInformation($"Retrieved {products.Count} products (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");
            return Ok(GenericResult<PagedResult<ProductDTO>>.Success(new PagedResult<ProductDTO>(products, page, pageSize, result.Value.TotalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<ProductDTO>>> GetProductById(int id)
        {
            var result = await _productService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning($"Product with id {id} was not found."); 
                return NotFound(GenericResult<ProductDTO>.Failure("Product not found."));
            }
            _logger.LogInformation($"Product with id {id} was retrieved successfully.");
            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(result.Value)));
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
                
                 var categoryExists = await _productService.CategoryExistsAsync(productDto.CategoryId);
                if(!categoryExists) {
                    _logger.LogWarning($"Category with id {productDto.CategoryId} does not exist.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("Invalid category."));
                }


                var product = _mapper.Map<Product>(productDto);

                _logger.LogInformation($"Creating product with name: {product.Name}, price: {product.Price}, categoryId: {product.CategoryId}.");
                var createResult = await _productService.CreateAsync(product);
                if (createResult.IsFailure)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<ProductDTO>.Failure(createResult.Error));
                }
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

            var getResult = await _productService.GetByIdAsync(id);

            if (getResult.IsFailure)  
            {
                _logger.LogWarning($"Product with id {id} was not found for update.");
                return NotFound(GenericResult<ProductDTO>.Failure($"Product with Id = {id} not found."));
            }

            var product = getResult.Value;

            if (!string.IsNullOrWhiteSpace(request.NewProductName))
            {
                bool exists = await _productService.ProductNameExistsAsync(request.NewProductName, id);

                if (exists)
                {

                    _logger.LogWarning($"A product with the name {request.NewProductName} already exists.");
                    return BadRequest(GenericResult<ProductDTO>.Failure("A product with this name already exists."));
                }

                //product.Name = request.NewProductName;
            }

            if (request.NewCategoryId.HasValue && request.NewCategoryId > 0)
            {
                var categoryExists = await _productService.CategoryExistsAsync(request.NewCategoryId.Value);

                if (!categoryExists)
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
            var updateResult = await _productService.UpdateAsync(product);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<ProductDTO>.Failure(updateResult.Error));
            }
            _logger.LogInformation($"Product with id {id} was updated successfully in the database.");
            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(product)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<ProductDTO>>> DeleteProduct(int id)
        {
            var deleteResult = await _productService.DeleteAsync(id);

            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Product with id {id} was not found for deletion.");
                return NotFound(GenericResult<ProductDTO>.Failure($"Product with Id = {id} not found."));
            }

            _logger.LogInformation($"Product with id {id} was deleted successfully from the database.");

            return Ok(GenericResult<ProductDTO>.Success(_mapper.Map<ProductDTO>(deleteResult.Value)));
        }
    }
}