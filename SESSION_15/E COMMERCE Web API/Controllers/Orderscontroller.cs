using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs;
using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrdersController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _context.Orders
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET api/orders/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderWithDetailsDto>> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return NotFound($"Order with id {id} was not found.");

            var dto = new OrderWithDetailsDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name ?? string.Empty,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.Product?.Name,
                    Quantity = od.Quantity
                })
            };

            return Ok(dto);
        }

        // GET api/orders/customer/{customerId}
        [HttpGet("customer/{customerId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(int customerId)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == customerId);
            if (!customerExists)
                return NotFound($"Customer with id {customerId} was not found.");

            var orders = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
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
                return NotFound($"Customer with id {dto.CustomerId} was not found.");

            // Validate all product ids up front
            var productIds = dto.OrderDetails.Select(od => od.ProductId).Distinct().ToList();
            var foundProducts = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            var missingProducts = productIds.Except(foundProducts).ToList();
            if (missingProducts.Any())
                return NotFound($"Products not found: {string.Join(", ", missingProducts)}");

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = dto.OrderDate ?? DateTime.UtcNow,
                OrderDetails = dto.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var responseDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId
            };

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, responseDto);
        }

        // PUT api/orders/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDto dto)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null)
                return NotFound($"Order with id {id} was not found.");

            if (dto.OrderDate is not null) order.OrderDate = dto.OrderDate.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/orders/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
                return NotFound($"Order with id {id} was not found.");

            _context.Orders.Remove(order); // cascade will handle OrderDetails
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}