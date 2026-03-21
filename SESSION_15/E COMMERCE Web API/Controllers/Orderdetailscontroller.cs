using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;

namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/orders/{orderId:int}/details")]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrderDetailsController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET api/orders/{orderId}/details
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetAll(int orderId)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderId);
            if (!orderExists)
                return NotFound($"Order with id {orderId} was not found.");

            var details = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    ProductName = od.Product != null ? od.Product.Name : null,
                    Quantity = od.Quantity
                })
                .ToListAsync();

            return Ok(details);
        }

        // GET api/orders/{orderId}/details/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDetailDto>> GetById(int orderId, int id)
        {
            var detail = await _context.OrderDetails
                .Include(od => od.Product)
                .FirstOrDefaultAsync(od => od.Id == id && od.OrderId == orderId);

            if (detail is null)
                return NotFound($"OrderDetail with id {id} was not found in order {orderId}.");

            var dto = new OrderDetailDto
            {
                Id = detail.Id,
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                ProductName = detail.Product?.Name,
                Quantity = detail.Quantity
            };

            return Ok(dto);
        }

        // POST api/orders/{orderId}/details
        [HttpPost]
        public async Task<ActionResult<OrderDetailDto>> Create(int orderId, CreateOrderDetailDto dto)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderId);
            if (!orderExists)
                return NotFound($"Order with id {orderId} was not found.");

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product is null)
                return NotFound($"Product with id {dto.ProductId} was not found.");

            var detail = new OrderDetail
            {
                OrderId = orderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.OrderDetails.Add(detail);
            await _context.SaveChangesAsync();

            var responseDto = new OrderDetailDto
            {
                Id = detail.Id,
                OrderId = detail.OrderId,
                ProductId = detail.ProductId,
                ProductName = product.Name,
                Quantity = detail.Quantity
            };

            return CreatedAtAction(nameof(GetById), new { orderId, id = detail.Id }, responseDto);
        }

        // PUT api/orders/{orderId}/details/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int orderId, int id, UpdateOrderDetailDto dto)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.Id == id && od.OrderId == orderId);

            if (detail is null)
                return NotFound($"OrderDetail with id {id} was not found in order {orderId}.");

            if (dto.Quantity is not null) detail.Quantity = dto.Quantity.Value;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/orders/{orderId}/details/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int orderId, int id)
        {
            var detail = await _context.OrderDetails
                .FirstOrDefaultAsync(od => od.Id == id && od.OrderId == orderId);

            if (detail is null)
                return NotFound($"OrderDetail with id {id} was not found in order {orderId}.");

            _context.OrderDetails.Remove(detail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}