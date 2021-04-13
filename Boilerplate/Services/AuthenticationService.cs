using Application.Models.Entities.Authentication;
using Application.Models.InputModels;
using Application.Models.Settings;
using Application.Models.ViewModels;
using Business.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class AuthenticationService
    {
        private readonly IAuthenticationBusiness authenticationBusiness;
        private readonly IConfiguration configuration;

        public AuthenticationService(IConfiguration configuration, IAuthenticationBusiness authenticationBusiness)
        {
            this.configuration = configuration;
            this.authenticationBusiness = authenticationBusiness;
        }

        public async Task<LoginResultViewModel> LoginAsync(LoginInputModel loginCredentials)
        {

            var loginResult = await authenticationBusiness.LoginAsync(loginCredentials.Username, loginCredentials.Password);
            var response = LoginResultViewModel.Parse(loginResult);

            if (loginResult.IsApproved)
            {
                var claims = RegistrerClaims(loginResult.User, loginResult.User.Roles);
                response.Token = GenerateToken(claims);
            }

            return response;
        }

        private static ClaimsIdentity RegistrerClaims(User user, List<Role> roles)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            return claims;
        }

        private string GenerateToken(ClaimsIdentity claims)
        {
            var settings = configuration.GetSection(nameof(AuthenticationSettings)).Get<AuthenticationSettings>();
            var secretKey = Encoding.ASCII.GetBytes(settings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(settings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            if (securityToken == null)
                return string.Empty;

            return tokenHandler.WriteToken(securityToken);
        }
    }
}
