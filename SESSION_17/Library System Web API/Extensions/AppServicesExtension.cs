using Library_System_Web_API.Data;
using Library_System_Web_API.Data.Interceptors;
using Library_System_Web_API.MappingHelper;
using Library_System_Web_API.Repositories;
using Library_System_Web_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Extensions
{
    public static class AppServicesExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        return new BadRequestObjectResult(context.ModelState);
                    };
                });
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            services.AddDbContext<LibraryDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(new SoftDeleteInterceptor())
                    .AddInterceptors(new CreatedAtInterceptor())
                    .AddInterceptors(new CreatedByInterceptor())
                    .AddInterceptors(new ModifiedAtInterceptor())
                    .AddInterceptors(new ModifiedByInterceptor()));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBorrowerService, BorrowerService>();
            services.AddScoped<ILoanService, LoanService>();

            return services;
        }
    }
}
