using Microsoft.EntityFrameworkCore;
using TinyCRM.Infrastructure;

namespace TinyCRM.API.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("TinyCRM.API"));
            }
            );

            return services;
        }
    }
}
