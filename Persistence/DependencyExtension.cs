using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Implementations;
using Persistence.Interface;

namespace Persistence
{
    public static class DependencyExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = string.Empty;
            connectionString = configuration.GetConnectionString("default");

            services.AddDbContext<Context>(options => options.UseMySql(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
