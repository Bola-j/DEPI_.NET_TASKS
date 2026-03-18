using E_Commerce_System.Data;
using E_Commerce_System.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Reflection.Metadata.BlobBuilder;


namespace E_Commerce_System
{
    internal class Program
    {
        static void Main(string[] args)
        {



            var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=E_COMMERCE_SYSTEM_DB;Trusted_Connection=True;TrustServerCertificate=True;");

            using var context = new ECommerceDbContext(optionsBuilder.Options);




            #region seed data (Create)
            var electronics = new Category { Name = "Electronics" };
            var clothing = new Category { Name = "Clothing" };
            var sports = new Category { Name = "Sports" };
            var books = new Category { Name = "Books" };

            context.Categories.AddRange(electronics, clothing, sports, books);
            context.SaveChanges();

            var laptop = new Product { Name = "Laptop", Price = 1200, CategoryId = electronics.Id };
            var smartphone = new Product { Name = "Smartphone", Price = 800, CategoryId = electronics.Id };
            var tshirt = new Product { Name = "T-Shirt", Price = 20, CategoryId = clothing.Id };
            var jeans = new Product { Name = "Jeans", Price = 50, CategoryId = clothing.Id };
            var runningSneakers = new Product { Name = "Running Sneakers", Price = 100, CategoryId = sports.Id };
            var basketball = new Product { Name = "Basketball", Price = 30, CategoryId = sports.Id };
            var novel = new Product { Name = "Novel", Price = 15, CategoryId = books.Id };

            context.Products.AddRange(laptop, smartphone, tshirt, jeans, runningSneakers, basketball, novel);
            context.SaveChanges();

            var alice = new Customer { Name = "Alice", Email = "alice@example.com" };
            var bob = new Customer { Name = "Bob", Email = "bob@example.com" };
            context.Customers.AddRange(alice, bob);
            context.SaveChanges();

            var order1 = new Order { CustomerId = alice.Id, OrderDate = DateTime.Now };
            var order2 = new Order { CustomerId = bob.Id, OrderDate = DateTime.Now };
            context.Orders.AddRange(order1, order2);
            context.SaveChanges();

            context.OrderDetails.AddRange(
                new OrderDetail { OrderId = order1.Id, ProductId = laptop.Id, Quantity = 1 },
                new OrderDetail { OrderId = order1.Id, ProductId = tshirt.Id, Quantity = 2 },
                new OrderDetail { OrderId = order2.Id, ProductId = smartphone.Id, Quantity = 1 },
                new OrderDetail { OrderId = order2.Id, ProductId = jeans.Id, Quantity = 1 }
            );
            context.SaveChanges();

            Console.WriteLine("Seeded successfully!");
            #endregion

            #region read data (Read)
            var orderedProducts = context.Products
                .Include(p => p.OrderDetails)
                .ThenInclude(od => od.Order)
                .Where(p => p.OrderDetails.Any());

            foreach (var item in orderedProducts.ToList())
            {
                Console.WriteLine($"{item.Name}, Oredered {item.OrderDetails.Sum(od => od.Quantity)} times");
            }

            Console.WriteLine();

            var unOrderedProducts = context.Products
                .Include(p => p.OrderDetails)
                .Where(p => !p.OrderDetails.Any());

            foreach (var item in unOrderedProducts.ToList())
            {
                Console.WriteLine($"{item.Name}, Oredered {item.OrderDetails.Sum(od => od.Quantity)} times");
            }
            #endregion

            Console.WriteLine();

            #region update data (Update)
            // update the orders of alice to be ordered by bob
            var customers = context.Customers.ToList();
            var orders = context.Orders.ToList();

            foreach (var order in orders)
            {
                if (order.CustomerId == customers[0].Id)
                {
                    order.CustomerId = customers[1].Id;
                    order.Customer = customers[1];
                }

            }

            context.UpdateRange(orders);
            context.SaveChanges();

            var ordersByCustomer = context.Orders
                .Include(o => o.Customer);
            Console.WriteLine();
            foreach (var order in ordersByCustomer.ToList())
            {
                Console.WriteLine($"Order : {order.Id}, Customer: {order.ToString()}, \n\t Customer: {order.Customer.ToString()}");
            }

            #endregion


            Console.WriteLine();


            #region delete data (Delete)
            // I want to delete a product with Id 3 without ruining whole order
            // deleting the product means that no order details will be there for it
            var products = context.Products.ToList();

            var orderDetails = context.OrderDetails.ToList();



            foreach (var item in orderDetails)
            {
                if (item.ProductId == 3)
                {
                    context.Remove(item);
                }
            }

            foreach (var product in products)
            {
                if (product.Id == 3)
                {

                    context.Remove(product);
                    break;

                }
            }
            context.SaveChanges();

            foreach (var item in context.OrderDetails.ToList())
            {
                Console.WriteLine($"Order Details: {item.ToString()}");
            }

            Console.WriteLine();

            foreach (var item in context.Products.ToList())
            {
                Console.WriteLine($"Order Details: {item.ToString()}");
            }

            #endregion


        }
    }
}
