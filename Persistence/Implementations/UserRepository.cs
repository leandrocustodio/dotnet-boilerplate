using Entities.Login;
using Microsoft.EntityFrameworkCore;
using Persistence.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistence.Implementations
{
    public class UserRepository : IUserRepository 
    {
        private readonly Context context;

        public UserRepository(Context context)
        {
            this.context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email.Equals(email));
            return user;
        }

        public async Task<List<Role>> ListRolesAsync(uint userId)
        {
            var roles = context.Roles.FromSqlInterpolated(@$"
                    SELECT * 
                    FROM 
                        user_roles ur 
                        JOIN roles r ON r.id = ur.role_id
                    WHERE
                        ur.user_id = {userId}");

            return await roles.ToListAsync();
        }

        public async Task MarkAsBlockedAsync(uint userId)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE users SET is_blocked = 1 WHERE id = {userId}");
        }

        public async Task SetUserAsActiveAsync(string userId)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE users SET is_active = 1 WHERE id = {userId}");
        }

        public async Task SetUserAsInactiveAsync(string userId)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE users SET is_active = 0 WHERE id = {userId}");
        }

        public async Task UpdateIncorrectAttemptsAsync(uint userId, int attempts)
        {
            await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE users SET incorrect_attempts = {attempts} WHERE id = {userId}");
        }
    }
}
