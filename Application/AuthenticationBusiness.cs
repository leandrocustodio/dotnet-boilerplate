using Business.Interface;
using Entities.Authentication;
using Entities.Login;
using Persistence.Interface;
using System.Threading.Tasks;

namespace Business
{
    public class AuthenticationBusiness : IAuthenticationBusiness
    {
        private readonly int maxAttempts = 5;
        private readonly IUserRepository userRepository;

        public AuthenticationBusiness(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
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
    }
}
