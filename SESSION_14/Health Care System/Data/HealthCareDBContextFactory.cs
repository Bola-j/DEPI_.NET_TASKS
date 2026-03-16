using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Care_System.Data
{
    internal class HealthCareDBContextFactory : IDesignTimeDbContextFactory<HealthCareDBContext>
    {
        public HealthCareDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HealthCareDBContext>();
            optionsBuilder.UseSqlServer(
                "Server=.;Database=HEALTH_CARE_SYSTEM_DB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new HealthCareDBContext(optionsBuilder.Options);
        }
    }
}
