using Microsoft.Extensions.DependencyInjection;

namespace InversionOfControl
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
