using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.Data;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        public AnalyticsController(ECommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet("orders/count")]
        public async Task<IActionResult> GetTotalNoOfOrders()
        {
            var totalOrders = await _context.Orders.CountAsync();
            return Ok(new { TotalOrders = totalOrders });
        }

        [HttpGet("orders/count/customer/{customerid:int}")]
        public async Task<IActionResult> GetTotalNoOfOrdersByCustomer(int customerid)
        {
            var totalOrders = await _context.Orders.CountAsync(o => o.CustomerId == customerid);
            return Ok(new { TotalOrders = totalOrders });
        }

        [HttpGet("orders/revenue")]
        public async Task<IActionResult> GetRevenue()
        {
            var revenue = await _context.OrderDetails
                .Include(od => od.Product)
                .SumAsync(od => od.Quantity * od.Product.Price);
            return Ok(new { Revenue = revenue });
        }

        [HttpGet("orders/revenue/customer/{customerid:int}")]
        public async Task<IActionResult> GetRevenueByCustomer(int customerid)
        {
            var revenue = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.Order.CustomerId == customerid)
                .SumAsync(od => od.Quantity * od.Product.Price);
            return Ok(new { Revenue = revenue });
        }
        [HttpGet("orders/average-value")]
        public async Task<IActionResult> GetAverageOrderValue()
        {
            var ordersValues = await _context.Orders
                .Select(o => o.OrderDetails.Sum(od => od.Quantity * od.Product.Price))
                .ToListAsync();
                
            var averageValue = ordersValues.Any() ? ordersValues.Average() : 0;
            return Ok(new { AverageOrderValue = averageValue });
        }

        [HttpGet("products/top-selling")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            var topProducts = await _context.OrderDetails
                .Include(od => od.Product)
                .GroupBy(od => new { od.ProductId, od.Product.Name })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(5)
                .ToListAsync();
            return Ok(topProducts);
        }

        [HttpGet("customers/top")]
        public async Task<IActionResult> GetTopCustomers()
        {
            var topCustomers = await _context.Orders
                .GroupBy(o => new { o.CustomerId, o.Customer.Name })
                .Select(g => new
                {
                    CustomerId = g.Key.CustomerId,
                    CustomerName = g.Key.Name,
                    TotalSpent = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity * od.Product.Price)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();
            return Ok(topCustomers);
        }

    }
}
