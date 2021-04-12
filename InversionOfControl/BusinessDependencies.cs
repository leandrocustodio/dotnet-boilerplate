using Microsoft.Extensions.DependencyInjection;
using System;

namespace InversionOfControl
{
    public static class BusinessDependencies
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            return services;
        }
    }
}
