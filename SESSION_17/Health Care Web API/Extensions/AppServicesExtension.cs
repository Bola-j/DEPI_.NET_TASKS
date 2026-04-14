using Health_Care_Web_API.Data;
using Health_Care_Web_API.Data.Interceptors;
using Health_Care_Web_API.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_Web_API.Extensions
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

            services.AddDbContext<HEALTH_CARE_SYSTEM_DBContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .AddInterceptors(
                        new SoftDeleteInterceptor(), new CreatedAtInterceptor(), new CreatedByInterceptor(), new ModifiedByInterceptor(), new ModifiedAtInterceptor()
                        )
                );
            return services;
        }
    }
}
