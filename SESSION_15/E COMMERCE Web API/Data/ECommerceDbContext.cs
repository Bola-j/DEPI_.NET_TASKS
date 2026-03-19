using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_COMMERCE_Web_API.Data
{
    using E_COMMERCE_Web_API.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ECommerceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }


        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=E_COMMERCE_SYSTEM_DB;Trusted_Connection=True;TrustServerCertificate=True;"
                );
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category -> Products (One to Many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 4);

            // Customer -> Orders (One to Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail Composite Key
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId });

            // OrderDetail -> Order
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // OrderDetail -> Product
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
