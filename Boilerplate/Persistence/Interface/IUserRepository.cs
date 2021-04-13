using Application.Models.Entities.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<List<Role>> ListRolesAsync(uint userId);
        Task UpdateIncorrectAttemptsAsync(uint userId, int attempts);
        Task MarkAsBlockedAsync(uint userId);
        Task SetUserAsInactiveAsync(string userId);
        Task SetUserAsActiveAsync(string userId);
    }
}
