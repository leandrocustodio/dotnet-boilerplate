using Application.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Application.Setup
{
    public static class Authentication
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(AuthenticationSettings)).Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(settings.SecretKey);

            services.AddAuthorization(options =>
            {
                AddPolicy(options, Roles.Admin, settings.ClaimsNamespace, Roles.Admin);
                AddPolicy(options, Roles.Manager, settings.ClaimsNamespace, Roles.Manager);
                AddPolicy(options, Roles.Seller, settings.ClaimsNamespace, Roles.Seller);
            })
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSingleton(settings);

            return services;
        }

        private static void AddPolicy(AuthorizationOptions options, string policyName, string claimNamespace, string role)
        {
            options.AddPolicy(policyName, policy => policy.RequireClaim(claimNamespace, role));
        }
    }
}
