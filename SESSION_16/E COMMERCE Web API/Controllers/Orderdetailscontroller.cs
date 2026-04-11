using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using AutoMapper;
using E_COMMERCE_Web_API.Results;
namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId:int}/details")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<OrderDetailsController> _logger;
        private readonly IMapper _mapper;

        public OrderDetailsController(ECommerceDbContext context, ILogger<OrderDetailsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET api/orders/{orderId}/details
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<OrderDetailDto>>>> GetAll(int page = 1, int pageSize = 10)
        {
            if(page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}");
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            var query = _context.OrderDetails
                .Include(od => od.Order)
                .Include(od => od.Product)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var details = await query
                .OrderBy(od => od.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(od => _mapper.Map<OrderDetailDto>(od))
                .ToListAsync();

            return Ok(GenericResult<PagedResult<OrderDetailDto>>.Success(new PagedResult<OrderDetailDto>(details, page, pageSize, totalCount)));
        }

        // GET api/orders/{orderId}/details/{productId}
        [HttpGet("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> GetById(int orderId, int productId)
        {
            var detail = await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail is null)
            {
                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            var dto = _mapper.Map<OrderDetailDto>(detail);

            return Ok(GenericResult<OrderDetailDto>.Success(dto));
        }

        // POST api/orders/{orderId}/details
        [HttpPost]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Create(int orderId, CreateOrderDetailDto dto)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderId);
            if (!orderExists)
            {
                _logger.LogInformation($"Order with id {orderId} was not found.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"Order with id {orderId} was not found."));
            }
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product is null)
            {
                _logger.LogInformation($"Product with id {dto.ProductId} was not found.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"Product with id {dto.ProductId} was not found."));
            }

            var exists = await _context.OrderDetails
                .AnyAsync(od => od.OrderId == orderId && od.ProductId == dto.ProductId);
            if (exists)
            { 
                _logger.LogInformation($"OrderDetail for order {orderId} and product {dto.ProductId} already exists.");
                return Conflict(GenericResult<OrderDetailDto>.Failure($"OrderDetail for order {orderId} and product {dto.ProductId} already exists."));
            }

            var detail = _mapper.Map<OrderDetail>(dto);
            
            _logger.LogInformation($"Creating OrderDetail for order {orderId} and product {dto.ProductId} with quantity {dto.Quantity}.");
            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"OrderDetail for order {orderId} and product {dto.ProductId} created successfully.");

           

            _logger.LogInformation($"Returning created OrderDetail for order {orderId} and product {dto.ProductId}.");
            return CreatedAtAction(nameof(GetById), new { orderId, productId = detail.ProductId }, GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(detail)));
        }

        // PUT api/orders/{orderId}/details/{productId}
        [HttpPut("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Update(int orderId, int productId, UpdateOrderDetailDto dto)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail is null)
            {
                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            if (dto.Quantity is null || dto.Quantity == 0)
            { 
                _logger.LogInformation($"Quantity is required and must be greater than 0 for updating OrderDetail with product id {productId} in order {orderId}.");
                return BadRequest(GenericResult<OrderDetailDto>.Failure($"Quantity is required and must be greater than 0 for updating OrderDetail with product id {productId} in order {orderId}."));
            }

            _mapper.Map(dto, detail);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"OrderDetail with product id {productId} in order {orderId} updated successfully.");

            return Ok(GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(detail)));
        }

        // DELETE api/orders/{orderId}/details/{productId}
        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<GenericResult<OrderDetailDto>>> Delete(int orderId, int productId)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.OrderId == orderId && od.ProductId == productId);

            if (detail is null)
            {

                _logger.LogInformation($"OrderDetail with product id {productId} was not found in order {orderId}.");
                return NotFound(GenericResult<OrderDetailDto>.Failure($"OrderDetail with product id {productId} was not found in order {orderId}."));
            }

            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"OrderDetail with product id {productId} in order {orderId} deleted successfully.");

            return Ok(GenericResult<OrderDetailDto>.Success(_mapper.Map<OrderDetailDto>(detail)));
        }
    }
}