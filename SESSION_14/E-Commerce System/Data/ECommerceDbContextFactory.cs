using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace E_Commerce_System.Data
{
    // EF Core will call this automatically when adding migrations
    internal class ECommerceDbContextFactory : IDesignTimeDbContextFactory<ECommerceDbContext>
    {
        public ECommerceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ECommerceDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=.;Database=E_COMMERCE_SYSTEM_DB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new ECommerceDbContext(optionsBuilder.Options);
        }
    }
}