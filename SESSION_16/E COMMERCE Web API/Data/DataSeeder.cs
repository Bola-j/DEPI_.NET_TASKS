using E_COMMERCE_Web_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_COMMERCE_Web_API.Data
{
    public class DataSeeder
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<DataSeeder> _logger;

        // Base path where seed JSON files are located (resolved at runtime)
        // Supports both "SeedData" and the existing folder name "SeedDate".
        private static readonly string[] SeedDataDirectories =
        {
            Path.Combine(AppContext.BaseDirectory, "Data", "SeedData"),
            Path.Combine(AppContext.BaseDirectory, "Data", "SeedDate"),
            Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedData"),
            Path.Combine(Directory.GetCurrentDirectory(), "Data", "SeedDate")
        };

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DataSeeder(ECommerceDbContext context, ILogger<DataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Entry point: run all seeders in dependency order
        public async Task SeedAsync()
        {
            await SeedCategoriesAsync();
            await SeedProductsAsync();
            await SeedCustomersAsync();
            await SeedOrdersAsync();
            await SeedOrderDetailsAsync();
        }

        // ─── Categories ──────────────────────────────────────────────────────────

        private async Task SeedCategoriesAsync()
        {
            if (await _context.Categories.AnyAsync())
            {
                _logger.LogInformation("Categories table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(json, JsonOptions);

            if (categories is null || categories.Count == 0)
            {
                _logger.LogWarning("categories.json is empty or could not be deserialized.");
                return;
            }

            foreach (var category in categories)
                category.Id = 0;

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} categories.", categories.Count);
        }

        // ─── Products ────────────────────────────────────────────────────────────

        private async Task SeedProductsAsync()
        {
            if (await _context.Products.AnyAsync())
            {
                _logger.LogInformation("Products table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("products.json");

            // Intermediate DTO to avoid FK conflicts during deserialization
            var dtos = JsonSerializer.Deserialize<List<ProductSeedDto>>(json, JsonOptions);

            if (dtos is null || dtos.Count == 0)
            {
                _logger.LogWarning("products.json is empty or could not be deserialized.");
                return;
            }

            // Load real category IDs so we can map positional index → real DB id
            var categoryIds = await _context.Categories
                .OrderBy(c => c.Id)
                .Select(c => c.Id)
                .ToListAsync();

            if (!categoryIds.Any())
                throw new Exception("Categories not seeded properly before Products.");

            var products = dtos.Select(dto => new Product
            {
                Id          = 0,
                Name        = dto.Name,
                Price       = dto.Price,
                // dto.CategoryId is a 1-based position in the seed JSON;
                // map it to a real DB id (clamp to available range for safety)
                CategoryId  = categoryIds[Math.Min(dto.CategoryId - 1, categoryIds.Count - 1)]
            }).ToList();

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} products.", products.Count);
        }

        // ─── Customers ───────────────────────────────────────────────────────────

        private async Task SeedCustomersAsync()
        {
            if (await _context.Customers.AnyAsync())
            {
                _logger.LogInformation("Customers table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("customers.json");
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, JsonOptions);

            if (customers is null || customers.Count == 0)
            {
                _logger.LogWarning("customers.json is empty or could not be deserialized.");
                return;
            }

            foreach (var customer in customers)
                customer.Id = 0;

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} customers.", customers.Count);
        }

        // ─── Orders ──────────────────────────────────────────────────────────────

        private async Task SeedOrdersAsync()
        {
            if (await _context.Orders.AnyAsync())
            {
                _logger.LogInformation("Orders table already has data — skipping seed.");
                return;
            }

            var json = await ReadJsonFileAsync("orders.json");
            var orders = JsonSerializer.Deserialize<List<Order>>(json, JsonOptions);

            if (orders is null || orders.Count == 0)
            {
                _logger.LogWarning("orders.json is empty or could not be deserialized.");
                return;
            }

            var customerIds = await _context.Customers
                .Select(c => c.Id)
                .ToListAsync();

            if (!customerIds.Any())
                throw new Exception("Customers not seeded properly before Orders.");

            var random = new Random();

            foreach (var order in orders)
            {
                order.Id         = 0;
                order.CustomerId = customerIds[random.Next(customerIds.Count)];
                order.Customer   = null;
            }

            _context.ChangeTracker.Clear();

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} orders.", orders.Count);
        }

        // ─── Order Details ───────────────────────────────────────────────────────

        private async Task SeedOrderDetailsAsync()
        {
            if (await _context.OrderDetails.AnyAsync())
            {
                _logger.LogInformation("OrderDetails table already has data — skipping seed.");
                return;
            }

            var orderIds   = await _context.Orders.Select(o => o.Id).ToListAsync();
            var productIds = await _context.Products.Select(p => p.Id).ToListAsync();

            if (!orderIds.Any() || !productIds.Any())
                throw new Exception("Orders or Products not seeded properly before OrderDetails.");

            var random      = new Random();
            var orderDetails = new List<OrderDetail>();
            var usedPairs   = new HashSet<(int, int)>();

            foreach (var orderId in orderIds)
            {
                // Each order gets 1–3 distinct products
                var lineCount = random.Next(1, 4);
                var shuffled  = productIds.OrderBy(_ => random.Next()).Take(lineCount);

                foreach (var productId in shuffled)
                {
                    if (usedPairs.Contains((orderId, productId)))
                        continue;

                    usedPairs.Add((orderId, productId));

                    orderDetails.Add(new OrderDetail
                    {
                        OrderId   = orderId,
                        ProductId = productId,
                        Quantity  = random.Next(1, 6)
                    });
                }
            }

            _context.ChangeTracker.Clear();

            await _context.OrderDetails.AddRangeAsync(orderDetails);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeded {Count} order details.", orderDetails.Count);
        }

        // ─── Helper ─────────────────────────────────────────────────────────────

        private static async Task<string> ReadJsonFileAsync(string fileName)
        {
            var filePath = SeedDataDirectories
                .Select(directory => Path.Combine(directory, fileName))
                .FirstOrDefault(File.Exists);

            if (filePath is null)
                throw new FileNotFoundException(
                    $"Seed data file not found: {fileName}. Checked: {string.Join("; ", SeedDataDirectories)}");

            return await File.ReadAllTextAsync(filePath);
        }

        // ─── Seed DTOs ──────────────────────────────────────────────────────────

        // Used only for deserialization — keeps the CategoryId as a positional
        // index without triggering EF navigation-property issues.
        private sealed class ProductSeedDto
        {
            public string  Name        { get; set; } = string.Empty;
            public decimal Price       { get; set; }
            public int     CategoryId  { get; set; }
        }
    }
}
