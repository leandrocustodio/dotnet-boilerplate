using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Setup
{
    public static class Services
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services)
        {
            return services;
        }
    }
}
