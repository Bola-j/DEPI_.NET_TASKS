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
                    "Name=DefaultConnection"
                );
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);
        }
    }
}
