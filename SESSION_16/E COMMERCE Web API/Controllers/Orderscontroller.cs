using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
using AutoMapper;
using E_COMMERCE_Web_API.DTOs;
using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ECommerceDbContext context, IMapper mapper, ILogger<OrdersController> logger)
        {
            _context = context;
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
            var query = _context.Orders
                .Include(o => o.Customer)
                .AsNoTracking();
            var searchTerm = search?.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm)) 
            { 
                query = query.Where(o => EF.Functions.Like(o.Customer.Name.ToLower(), $"%{searchTerm}%"));
            }

            var totalCount = await query.CountAsync();
            
            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => _mapper.Map<OrderDto>(o))
                .ToListAsync();


            return Ok(GenericResult<PagedResult<OrderDto>>.Success(new PagedResult<OrderDto>(orders, page, pageSize, totalCount)));
        }

        // GET api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderWithDetailsDto>>> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {

                _logger.LogWarning($"Order with id {id} was not found.");
                return NotFound(GenericResult<OrderWithDetailsDto>.Failure($"Order with id {id} was not found."));
            }

            var dto = _mapper.Map<OrderWithDetailsDto>(order);

            return Ok(GenericResult<OrderWithDetailsDto>.Success(dto));
        }

        // GET api/orders/customer/{customerId}
        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<GenericResult<IEnumerable<OrderDto>>>> GetByCustomer(int customerId)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
            {
                _logger.LogWarning($"Customer with id {customerId} was not found.");
                return NotFound(GenericResult<IEnumerable<OrderDto>>.Failure($"Customer with id {customerId} was not found."));
            }

            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
                .ToListAsync();

            return Ok(orders);
        }

        // POST api/orders
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto dto)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == dto.CustomerId);
            if (!customerExists)
            {
                _logger.LogWarning($"Customer with id {dto.CustomerId} was not found.");
                return NotFound(GenericResult<OrderDto>.Failure($"Customer with id {dto.CustomerId} was not found."));
            }

            // Validate all product ids up front
            var productIds = dto.OrderDetails.Select(od => od.ProductId).Distinct().ToList();
            var missingProducts = await _context.Products
                .Where(p => !productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();
            //var missingProducts = productIds.Except(foundProducts).ToList();
            if (missingProducts.Any())
            {
                _logger.LogWarning($"Products not found: {string.Join(", ", missingProducts)}");
                return NotFound(GenericResult<OrderDto>.Failure($"Products not found: {string.Join(", ", missingProducts)}"));
            }

            var order = _mapper.Map<Order>(dto);

            _logger.LogInformation($"Creating new order for customer {dto.CustomerId} with {dto.OrderDetails.Count} details.");
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order created with id {order.Id}.");


            var responseDto = _mapper.Map<OrderDto>(order);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, GenericResult<OrderDto>.Success(responseDto));
        }

        // PUT api/orders/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderDto>>> Update(int id, UpdateOrderDto dto)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
            { 
                return NotFound(GenericResult<OrderDto>.Failure($"Order with id {id} was not found."));
            }

            if (!(dto.OrderDate is not null && dto.OrderDate.HasValue && dto.OrderDate.Value <= DateTime.Now))
            {
                _logger.LogWarning($"Invalid OrderDate for order {id}: {dto.OrderDate}");
                return BadRequest(GenericResult<OrderDto>.Failure("OrderDate must be a valid date in the past or present."));
            }

            _mapper.Map(dto, order);
            _logger.LogInformation($"Updating order {id} with new OrderDate: {dto.OrderDate}");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order {id} updated successfully.");
            return Ok(GenericResult<OrderDto>.Success(_mapper.Map<OrderDto>(order)));
        }

        // DELETE api/orders/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenericResult<OrderDto>>> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                _logger.LogWarning($"Order with id {id} was not found for deletion.");
                return NotFound(GenericResult<OrderDto>.Failure($"Order with id {id} was not found."));
            }


            _context.Orders.Remove(order); // cascade will handle OrderDetails
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Order with id {id} was deleted successfully.");

            return Ok(GenericResult<OrderDto>.Success(_mapper.Map<OrderDto>(order)));
        }
    }
}