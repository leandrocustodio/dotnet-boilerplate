using Entities.Authentication;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IAuthenticationBusiness
    {
        Task<LoginResult> LoginAsync(string username, string password);
    }
}
