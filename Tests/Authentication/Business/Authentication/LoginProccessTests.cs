using Application.Models.Entities.Authentication;
using Business;
using Moq;
using Persistence.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Authentication.Business.Authentication
{
    public class LoginProccessTests
    {
        private readonly User user;
        private readonly string password;
        private readonly List<Role> roles;
        private readonly Mock<IUserRepository> userRepository;

        public LoginProccessTests()
        {
            userRepository = new Mock<IUserRepository>();
            password = "senha";
            user = new User("name", "lastname", "email", password);
            roles = new List<Role>()
            {
                new Role() { Id = "abc-ede", Name = "231" }
            };
        }

        [Fact]
        public async Task WhenLoginIsValid_ShouldReturnCorrectResult()
        {
            userRepository.Setup(x => x.GetByEmailAsync(user.Email))
                          .ReturnsAsync(user);

            userRepository.Setup(x => x.ListRolesAsync(user.Id))
              .ReturnsAsync(roles);

            var authBusiness = new AuthenticationBusiness(null, userRepository.Object);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, password);

            Assert.True(loginResult.IsApproved);
            Assert.Equal(loginResult.User, user);
            Assert.Equal(loginResult.User.Roles, roles);
        }

        [Fact]
        public async Task WhenLoginIsIncorrect_ShouldIncreaseTheNumberOfAttempts()
        {
            userRepository.Setup(x => x.GetByEmailAsync(user.Email))
                          .ReturnsAsync(user);

            userRepository.Setup(x => x.UpdateIncorrectAttemptsAsync(user.Id, It.IsAny<int>()));

            var expectedAttempts = user.IncorrectAttempts + 1;
            var authBusiness = new AuthenticationBusiness(null, userRepository.Object);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, "wrong_password");

            Assert.False(loginResult.IsApproved);
            Assert.True(loginResult.IncorrectUsernameOrPassword);
            Assert.Equal(expectedAttempts, user.IncorrectAttempts);
        }

        [Fact]
        public async Task WhenUserExceedTheLimitOfAttempts_ShouldBlockTheUser()
        {
            userRepository.Setup(x => x.GetByEmailAsync(user.Email))
                          .ReturnsAsync(user);

            userRepository.Setup(x => x.UpdateIncorrectAttemptsAsync(user.Id, It.IsAny<int>()));
            userRepository.Setup(x => x.MarkAsBlockedAsync(user.Id));

            user.IncorrectAttempts = 4;
            var expectedAttempts = user.IncorrectAttempts + 1;

            var authBusiness = new AuthenticationBusiness(null, userRepository.Object);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, "wrong_password");

            Assert.False(loginResult.IsApproved);
            Assert.True(loginResult.IncorrectUsernameOrPassword);
            Assert.True(loginResult.IsUserBlocked);
            Assert.Equal(expectedAttempts, user.IncorrectAttempts);
        }
    }
}
