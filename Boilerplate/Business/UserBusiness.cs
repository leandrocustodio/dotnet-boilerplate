using Application.Business.Interface;
using Application.Models.Entities.Authentication;
using Persistence.Interface;
using System;
using System.Threading.Tasks;

namespace Application.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateAsync(User user, string roleId)
        {
            await _userRepository.CreateAsync(user, roleId);
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _userRepository.ExistsAsync(email);
        }
    }
}
