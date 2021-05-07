using Application.Models.InputModels;
using Application.Models.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IAuthenticationBusiness
    {
        Task<LoginResultViewModel> LoginAsync(LoginInputModel loginCredentials);

        string RefreshToken(IEnumerable<Claim> claims);
    }
}
