using Application.Models.Entities.Authentication;
using Application.Models.InputModels;
using Application.Models.Settings;
using Application.Models.ViewModels;
using Business.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly int maxAttempts = 5;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public AuthenticationBusiness(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        public async Task<LoginResultViewModel> LoginAsync(LoginInputModel loginCredentials)
        {

            var loginResult = await CheckCredentialsAsync(loginCredentials.Username, loginCredentials.Password);
            var response = LoginResultViewModel.Parse(loginResult);

            if (loginResult.IsApproved)
            {
                var claims = RegistrerClaims(loginResult.User, loginResult.User.Roles);
                response.Token = GenerateToken(claims);
            }

            return response;
        }


        public async Task<LoginResult> CheckCredentialsAsync(string username, string password)
        {
            var user = await userRepository.GetByEmailAsync(username);
            var loginResult = ValidateLogin(user, password);

            if (loginResult.IsApproved)
            {
                user.Roles = await userRepository.ListRolesAsync(user.Id);
                loginResult.User = user;
            }

            if (loginResult.IncorrectUsernameOrPassword)
            {
                user.IncorrectAttempts++;
                await userRepository.UpdateIncorrectAttemptsAsync(user.Id, user.IncorrectAttempts);

                if (user.IncorrectAttempts >= maxAttempts)
                {
                    await userRepository.MarkAsBlockedAsync(user.Id);
                    loginResult.IsUserBlocked = true;
                }
            }

            return loginResult;
        }

        public static LoginResult ValidateLogin(User user, string password)
        {
            var loginResult = new LoginResult();

            if (user == null)
                loginResult.IncorrectUsernameOrPassword = true;

            else if (user.IsActive == false)
                loginResult.IsUserInactive = true;

            else if (user.IsBlocked)
                loginResult.IsUserBlocked = true;

            else if (string.IsNullOrWhiteSpace(password))
                loginResult.IncorrectUsernameOrPassword = true;

            else if (user.IsPasswordCorrect(password) == false)
                loginResult.IncorrectUsernameOrPassword = true;

            return loginResult;
        }

        public string RefreshToken(IEnumerable<Claim> claims)
        {
            var claimIdentity = new ClaimsIdentity();
            claimIdentity.AddClaims(claims);

            return GenerateToken(claimIdentity);
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
                Expires = DateTime.Now.AddMinutes(settings.TokenLifeTime),
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
