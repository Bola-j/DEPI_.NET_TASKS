using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
using AutoMapper;
using E_COMMERCE_Web_API.DTOs;
using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.Results;
using E_COMMERCE_Web_API.Services;

namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<OrderDto>>>> GetAll(string? search, int page = 1, int pageSize = 10)
        {
            if(page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}");
                return BadRequest("Page and pageSize must be greater than 0.");
            }
            var result = await _orderService.GetAllAsync(search, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<OrderDto>>.Failure(result.Error));
            }

            var orders = result.Value.Data.Select(_mapper.Map<OrderDto>).ToList();


            return Ok(GenericResult<PagedResult<OrderDto>>.Success(new PagedResult<OrderDto>(orders, page, pageSize, result.Value.TotalCount)));
        }

        // GET api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderWithDetailsDto>>> GetById(int id)
        {
            var result = await _orderService.GetByIdAsync(id);

            if (result.IsFailure)
            {

                _logger.LogWarning($"Order with id {id} was not found.");
                return NotFound(GenericResult<OrderWithDetailsDto>.Failure($"Order with id {id} was not found."));
            }

            var dto = _mapper.Map<OrderWithDetailsDto>(result.Value);

            return Ok(GenericResult<OrderWithDetailsDto>.Success(dto));
        }

        // GET api/orders/customer/{customerId}
        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<GenericResult<OrderDto>>> GetByCustomer(int customerId)
        {
            var customerExists = await _orderService.CustomerExistsAsync(customerId);
            if (!customerExists)
            {
                _logger.LogWarning($"Customer with id {customerId} was not found.");
                return NotFound(GenericResult<IEnumerable<OrderDto>>.Failure($"Customer with id {customerId} was not found."));
            }

            var ordersResult = await _orderService.GetByCustomerAsync(customerId);
            if (ordersResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<IEnumerable<OrderDto>>.Failure(ordersResult.Error));
            }

            var orders = ordersResult.Value.Select(_mapper.Map<OrderDto>).ToList();

            return Ok(GenericResult<IEnumerable<OrderDto>>.Success(orders));
        }

        // POST api/orders
        [HttpPost]
        public async Task<ActionResult<GenericResult<OrderDto>>> Create(CreateOrderDto dto)
        {
            var customerExists = await _orderService.CustomerExistsAsync(dto.CustomerId);
            if (!customerExists)
            {
                _logger.LogWarning($"Customer with id {dto.CustomerId} was not found.");
                return NotFound(GenericResult<OrderDto>.Failure($"Customer with id {dto.CustomerId} was not found."));
            }

            // Validate all product ids up front
            var productIds = dto.OrderDetails.Select(od => od.ProductId).Distinct().ToList();
            var missingProducts = await _orderService.GetMissingProductIdsAsync(productIds);
            //var missingProducts = productIds.Except(foundProducts).ToList();
            if (missingProducts.Any())
            {
                _logger.LogWarning($"Products not found: {string.Join(", ", missingProducts)}");
                return NotFound(GenericResult<OrderDto>.Failure($"Products not found: {string.Join(", ", missingProducts)}"));
            }

            var order = _mapper.Map<Order>(dto);

            _logger.LogInformation($"Creating new order for customer {dto.CustomerId} with {dto.OrderDetails.Count} details.");
            var createResult = await _orderService.CreateAsync(order);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<OrderDto>.Failure(createResult.Error));
            }
            _logger.LogInformation($"Order created with id {order.Id}.");


            var responseDto = _mapper.Map<OrderDto>(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, GenericResult<OrderDto>.Success(responseDto));
        }

        // PUT api/orders/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderDto>>> Update(int id, UpdateOrderDto dto)
        {
            var getResult = await _orderService.GetByIdAsync(id);

            if (getResult.IsFailure)
            { 
                return NotFound(GenericResult<OrderDto>.Failure($"Order with id {id} was not found."));
            }

            var order = getResult.Value;

            if (!(dto.OrderDate is not null && dto.OrderDate.HasValue && dto.OrderDate.Value <= DateTime.Now))
            {
                _logger.LogWarning($"Invalid OrderDate for order {id}: {dto.OrderDate}");
                return BadRequest(GenericResult<OrderDto>.Failure("OrderDate must be a valid date in the past or present."));
            }

            _mapper.Map(dto, order);
            _logger.LogInformation($"Updating order {id} with new OrderDate: {dto.OrderDate}");
            var updateResult = await _orderService.UpdateAsync(order);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<OrderDto>.Failure(updateResult.Error));
            }
            _logger.LogInformation($"Order {id} updated successfully.");
            return Ok(GenericResult<OrderDto>.Success(_mapper.Map<OrderDto>(order)));
        }

        // DELETE api/orders/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderDto>>> Delete(int id)
        {
            var deleteResult = await _orderService.DeleteAsync(id);

            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Order with id {id} was not found for deletion.");
                return NotFound(GenericResult<OrderDto>.Failure($"Order with id {id} was not found."));
            }


            _logger.LogInformation($"Order with id {id} was deleted successfully.");

            return Ok(GenericResult<OrderDto>.Success(_mapper.Map<OrderDto>(deleteResult.Value)));
        }
    }
}