using E_COMMERCE_Web_API.Converters;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.MappingHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E_COMMERCE_Web_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(context.ModelState);
                    };
                });
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            builder.Services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            builder.Services.AddControllers().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new NullNormalizingStringConverter());
                });

            builder.Services.AddScoped<DataSeeder>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var db = services.GetRequiredService<ECommerceDbContext>();
                    if (db.Database.GetMigrations().Any())
                    {
                        await db.Database.MigrateAsync();
                    }
                    else
                    {
                        await db.Database.EnsureCreatedAsync();

                        await db.Database.OpenConnectionAsync();
                        try
                        {
                            await using var command = db.Database.GetDbConnection().CreateCommand();
                            command.CommandText = "SELECT CASE WHEN OBJECT_ID(N'[dbo].[Categories]', N'U') IS NULL THEN 0 ELSE 1 END";

                            var categoriesExists = Convert.ToInt32(await command.ExecuteScalarAsync()) == 1;
                            if (!categoriesExists)
                            {
                                db.GetService<IRelationalDatabaseCreator>().CreateTables();
                            }
                        }
                        finally
                        {
                            await db.Database.CloseConnectionAsync();
                        }
                    }

                    var seeder = services.GetRequiredService<Data.DataSeeder>();
                    await seeder.SeedAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Migration/seeding failed.");
                }
            }

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
