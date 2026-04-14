
using E_COMMERCE_Web_API.Converters;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.Data.Interceptors;
using E_COMMERCE_Web_API.MappingHelper;
using E_COMMERCE_Web_API.Repositories;
using E_COMMERCE_Web_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Extensions
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

            services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(new SoftDeleteInterceptor())
                    .AddInterceptors(new CreatedAtInterceptor())
                    .AddInterceptors(new CreatedByInterceptor())
                    .AddInterceptors(new ModifiedAtInterceptor())
                    .AddInterceptors(new ModifiedByInterceptor())


                );

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new NullNormalizingStringConverter());
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();

            
            return services;
        }
    }
}
