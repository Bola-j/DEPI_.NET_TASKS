using Library_System_Web_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_System_Web_API.MappingHelper;

namespace Library_System_Web_API
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

            builder.Services.AddDbContext<LibraryDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<Data.DataSeeder>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var db = services.GetRequiredService<LibraryDBContext>();
                    await db.Database.MigrateAsync();

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
