using Application.Models.Entities.Authentication;
using Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Persistence.Interface;
using System;
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

        public async Task CreateAsync(User user, string roleId)
        {
            context.Users.Add(user);

            var userRole = new UserRole()
            {
                RoleId = roleId,
                UserId = user.Id
            };

            context.UserRoles.Add(userRole);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email.Equals(email));
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
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
