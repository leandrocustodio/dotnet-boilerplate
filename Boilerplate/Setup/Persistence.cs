using Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Implementations;
using Persistence.Interface;

namespace Application.Setup
{
    public static class Persistence
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = string.Empty;
            connectionString = configuration.GetConnectionString("default");

            services.AddDbContext<Context>(options => options
                //.UseLoggerFactory()
                .EnableSensitiveDataLogging()
                .UseMySql(connectionString)
            );
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
