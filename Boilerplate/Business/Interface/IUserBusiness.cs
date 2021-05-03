using Application.Models.Entities.Authentication;
using System.Threading.Tasks;

namespace Application.Business.Interface
{
    public interface IUserBusiness
    {
        Task CreateAsync(User user, string roleId);
        Task<bool> ExistsAsync(string email);
    }
}
