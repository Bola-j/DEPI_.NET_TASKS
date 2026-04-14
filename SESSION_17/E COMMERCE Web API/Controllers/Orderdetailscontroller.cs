using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using AutoMapper;
using E_COMMERCE_Web_API.Results;
using E_COMMERCE_Web_API.Services;
namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId:int}/details")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderDetailsController> _logger;
        private readonly IMapper _mapper;

        public OrderDetailsController(IOrderDetailService orderDetailService, ILogger<OrderDetailsController> logger, IMapper mapper)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET api/orders/{orderId}/details
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<OrderDetailDto>>>> GetAll(int orderId, int page = 1, int pageSize = 10)
        {
            if(page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}");
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var result = await _orderDetailService.GetAllAsync(orderId, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<OrderDetailDto>>.Failure(result.Error));
            }

            var details = result.Value.Data.Select(_mapper.Map<OrderDetailDto>).ToList();

            return Ok(GenericResult<PagedResult<OrderDetailDto>>.Success(new PagedResult<OrderDetailDto>(details, page, pageSize, result.Value.TotalCount)));
        }

        // GET api/orders/{orderId}/details/{productId}
        [HttpGet("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> GetById(int orderId, int productId)
        {
            var result = await _orderDetailService.GetByIdAsync(orderId, productId);

            if (result.IsFailure)
            {
                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            var dto = _mapper.Map<OrderDetailDto>(result.Value);

            return Ok(GenericResult<OrderDetailDto>.Success(dto));
        }

        // POST api/orders/{orderId}/details
        [HttpPost]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Create(int orderId, CreateOrderDetailDto dto)
        {
            var orderExists = await _orderDetailService.OrderExistsAsync(orderId);
            if (!orderExists)
            {
                _logger.LogInformation($"Order with id {orderId} was not found.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"Order with id {orderId} was not found."));
            }
            var productExists = await _orderDetailService.ProductExistsAsync(dto.ProductId);
            if (!productExists)
            {
                _logger.LogInformation($"Product with id {dto.ProductId} was not found.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"Product with id {dto.ProductId} was not found."));
            }

            var exists = await _orderDetailService.ExistsAsync(orderId, dto.ProductId);
            if (exists)
            { 
                _logger.LogInformation($"OrderDetail for order {orderId} and product {dto.ProductId} already exists.");
                return Conflict(GenericResult<OrderDetailDto>.Failure($"OrderDetail for order {orderId} and product {dto.ProductId} already exists."));
            }

            var detail = _mapper.Map<OrderDetail>(dto);
            detail.OrderId = orderId;

            _logger.LogInformation($"Creating OrderDetail for order {orderId} and product {dto.ProductId} with quantity {dto.Quantity}.");
            var createResult = await _orderDetailService.CreateAsync(detail);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<OrderDetailDto>.Failure(createResult.Error));
            }
            _logger.LogInformation($"OrderDetail for order {orderId} and product {dto.ProductId} created successfully.");

           

            _logger.LogInformation($"Returning created OrderDetail for order {orderId} and product {dto.ProductId}.");
            return CreatedAtAction(nameof(GetById), new { orderId, productId = detail.ProductId }, GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(detail)));
        }

        // PUT api/orders/{orderId}/details/{productId}
        [HttpPut("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Update(int orderId, int productId, UpdateOrderDetailDto dto)
        {
            var getResult = await _orderDetailService.GetByIdAsync(orderId, productId);

            if (getResult.IsFailure)
            {
                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            var detail = getResult.Value;

            if (dto.Quantity is null || dto.Quantity == 0)
            { 
                _logger.LogInformation($"Quantity is required and must be greater than 0 for updating OrderDetail with product id {productId} in order {orderId}.");
                return BadRequest(GenericResult<OrderDetailDto>.Failure($"Quantity is required and must be greater than 0 for updating OrderDetail with product id {productId} in order {orderId}."));
            }

            _mapper.Map(dto, detail);
            var updateResult = await _orderDetailService.UpdateAsync(detail);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<OrderDetailDto>.Failure(updateResult.Error));
            }
            _logger.LogInformation($"OrderDetail with product id {productId} in order {orderId} updated successfully.");

            return Ok(GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(detail)));
        }

        // DELETE api/orders/{orderId}/details/{productId}
        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Delete(int orderId, int productId)
        {
            var deleteResult = await _orderDetailService.DeleteAsync(orderId, productId);

            if (deleteResult.IsFailure)
            {

                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            _logger.LogInformation($"OrderDetail with product id {productId} in order {orderId} deleted successfully.");

            return Ok(GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(deleteResult.Value)));
        }
    }
}