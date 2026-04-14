using Health_Care_Web_API.Data;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Health_Care_Web_API.Data.Interceptors;
using Health_Care_Web_API.Extensions;

namespace Health_Care_Web_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            
            
            builder.Services.AddAppServices(builder.Configuration);

            builder.Services.AddScoped<Data.DataSeeder>();

            // Auto Migrate and Seed Database on Startup



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var db = services.GetRequiredService<HEALTH_CARE_SYSTEM_DBContext>();
                    await db.Database.MigrateAsync();

                    var seeder = services.GetRequiredService<Data.DataSeeder>();
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Migration/seeding failed.");
                }
            }

            //// Run database seeder on startup
            //using (var scope = app.Services.CreateScope())
            //{
            //    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
            //    await seeder.SeedAsync();
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
