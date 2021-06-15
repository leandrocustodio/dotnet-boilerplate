using Application.Models.Entities.Authentication;
using Application.Models.Settings;
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
        private readonly AuthenticationSettings authenticationSettings;

        public LoginProccessTests()
        {
            password = "senha";
            user = new User("name", "lastname", "email", password);
            authenticationSettings = new AuthenticationSettings() { ClaimsNamespace = "bla", MaxAttempts = 5, SecretKey = "bla", TokenLifeTime = 2 };
            userRepository = SetupUserRepositoryMock();
            roles = new List<Role>()
            {
                new Role() { Id = "abc-ede", Name = "231" }
            };
        }

        private Mock<IUserRepository> SetupUserRepositoryMock()
        {
            var userRepositoryMock = new Mock<IUserRepository>();

            userRepositoryMock.Setup(x => x.GetByEmailAsync(user.Email))
              .ReturnsAsync(user);

            return userRepositoryMock;
        }

        [Fact]
        public async Task WhenLoginIsValid_ShouldReturnCorrectResult()
        {
            userRepository.Setup(x => x.ListRolesAsync(user.Id))
              .ReturnsAsync(roles);

            var authBusiness = new AuthenticationBusiness(userRepository.Object, authenticationSettings);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, password);

            Assert.True(loginResult.IsApproved);
            Assert.Equal(loginResult.User, user);
            Assert.Equal(loginResult.User.Roles, roles);
        }

        [Fact]
        public async Task WhenLoginIsIncorrect_ShouldIncreaseTheNumberOfAttempts()
        {
            userRepository.Setup(x => x.UpdateIncorrectAttemptsAsync(user.Id, It.IsAny<int>()));

            var expectedAttempts = user.IncorrectAttempts + 1;
            var authBusiness = new AuthenticationBusiness(userRepository.Object, authenticationSettings);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, "wrong_password");

            Assert.False(loginResult.IsApproved);
            Assert.True(loginResult.IncorrectUsernameOrPassword);
            Assert.Equal(expectedAttempts, user.IncorrectAttempts);
        }

        [Fact]
        public async Task WhenUserExceedTheLimitOfAttempts_ShouldBlockTheUser()
        {
            userRepository.Setup(x => x.UpdateIncorrectAttemptsAsync(user.Id, It.IsAny<int>()));
            userRepository.Setup(x => x.MarkAsBlockedAsync(user.Id));

            user.IncorrectAttempts = 4;
            var expectedAttempts = user.IncorrectAttempts + 1;

            var authBusiness = new AuthenticationBusiness(userRepository.Object, authenticationSettings);
            var loginResult = await authBusiness.CheckCredentialsAsync(user.Email, "wrong_password");

            Assert.False(loginResult.IsApproved);
            Assert.True(loginResult.IncorrectUsernameOrPassword);
            Assert.True(loginResult.IsUserBlocked);
            Assert.Equal(expectedAttempts, user.IncorrectAttempts);
        }
    }
}
