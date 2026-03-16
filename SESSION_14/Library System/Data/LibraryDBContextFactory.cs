using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Data
{
    internal class LibraryDBContextFactory : IDesignTimeDbContextFactory<LibraryDBContext>
    {
        public LibraryDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDBContext>();
            optionsBuilder.UseSqlServer(
                "Server=.;Database=LIBRARY_SYSTEM_DB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new LibraryDBContext(optionsBuilder.Options);
        }
        
    }
}
