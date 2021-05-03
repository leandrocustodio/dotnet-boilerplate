using Application.Business;
using Application.Business.Interface;
using Business;
using Business.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Setup
{
    public static class Business
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationBusiness, AuthenticationBusiness>();
            services.AddScoped<IUserBusiness, UserBusiness>();

            return services;
        }
    }
}
