using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using E_COMMERCE_Web_API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using E_COMMERCE_Web_API.Results;

namespace E_COMMERCE_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<AnalyticsController> _logger;
        private readonly IMapper _mapper;
        public AnalyticsController(ECommerceDbContext context, ILogger<AnalyticsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("orders/count")]
        public async Task<ActionResult<GenericResult<int>>> GetTotalNoOfOrders()
        {
            var totalOrders = await _context.Orders.CountAsync();
            _logger.LogInformation($"Total number of orders is {totalOrders}.");

            return Ok(GenericResult<int>.Success(totalOrders));
        }

        [HttpGet("orders/count/customer/{customerid:int}")]
        public async Task<ActionResult<GenericResult<int>>> GetTotalNoOfOrdersByCustomer(int customerid)
        {
            var totalOrders = await _context.Orders.CountAsync(o => o.CustomerId == customerid);

            _logger.LogInformation($"Total number of orders for customer with id {customerid} is {totalOrders}.");
            return Ok(GenericResult<int>.Success(totalOrders));
        }

        [HttpGet("orders/revenue")]
        public async Task<ActionResult<GenericResult<decimal>>> GetRevenue()
        {
            var revenue = await _context.OrderDetails
                .Include(od => od.Product)
                .SumAsync(od => od.Quantity * od.Product.Price);

            _logger.LogInformation($"Total revenue from orders is {revenue}.");

            return Ok(GenericResult<decimal>.Success(revenue));
        }

        [HttpGet("orders/revenue/customer/{customerid:int}")]
        public async Task<ActionResult<GenericResult<decimal>>> GetRevenueByCustomer(int customerid)
        {
            var revenue = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.Order.CustomerId == customerid)
                .SumAsync(od => od.Quantity * od.Product.Price);

            _logger.LogInformation($"Total revenue from orders for customer with id {customerid} is {revenue}."); 
            return Ok(GenericResult<decimal>.Success(revenue));
        }
        [HttpGet("orders/average-value")]
        public async Task<ActionResult<GenericResult<decimal>>> GetAverageOrderValue()
        {
            var ordersValues = await _context.Orders
                .Select(o => o.OrderDetails.Sum(od => od.Quantity * od.Product.Price))
                .ToListAsync();
            var averageValue = ordersValues.Any() ? ordersValues.Average() : 0;
            _logger.LogInformation($"Average order value calculated from {ordersValues.Count} orders.");
            _logger.LogInformation($"Average order value is {averageValue}.");
            return Ok(GenericResult<decimal>.Success(averageValue));
        }

        [HttpGet("products/top-selling")]
        public async Task<ActionResult<GenericResult<IEnumerable<object>>>> GetTopSellingProducts()
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

            _logger.LogInformation($"Top 5 selling products retrieved.");
            return Ok(GenericResult<IEnumerable<object>>.Success(topProducts));
        }

        [HttpGet("customers/top")]
        public async Task<ActionResult<GenericResult<IEnumerable<object>>>> GetTopCustomers()
        {
            var topCustomers = await _context.Orders
                .GroupBy(o => new { o.CustomerId, o.Customer.Name })
                .Select(g => new
                {
                    CustomerName = g.Key.Name,
                    TotalSpent = g.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity * od.Product.Price)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();
            _logger.LogInformation($"Top 5 customers retrieved based on total spending.");
            return Ok(GenericResult<IEnumerable<object>>.Success(topCustomers));
        }

    }
}
