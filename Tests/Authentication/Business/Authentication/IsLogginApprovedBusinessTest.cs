using Business;
using Entities.Login;
using System;
using Xunit;

namespace Tests.Business.Authentication
{
    public class IsLogginApprovedBusinessTest
    {
        [Fact]
        public void ValidateLoginWhenUserIsNull_ExpectIncorrectUsernameOrPassword()
        {
            User user = null;

            var currentResult = AuthenticationBusiness.ValidateLogin(user, string.Empty);

            Assert.True(currentResult.IncorrectUsernameOrPassword);
            Assert.False(currentResult.IsApproved);
        }

        [Fact]
        public void ValidateLoginWhenUserIsInative_ExpectIsUserInactive()
        {
            var user = new User()
            {
                IsActive = false
            };

            var currentResult = AuthenticationBusiness.ValidateLogin(user, string.Empty);

            Assert.True(currentResult.IsUserInactive);
            Assert.False(currentResult.IsApproved);
        }

        [Fact]
        public void ValidateLoginWhenUserIsBlocked_ExpectIsUserBlocked()
        {
            var user = new User()
            {
                IsActive = true,
                IsBlocked = true
            };

            var currentResult = AuthenticationBusiness.ValidateLogin(user, string.Empty);

            Assert.True(currentResult.IsUserBlocked);
            Assert.False(currentResult.IsApproved);
        }

        [Fact]
        public void ValidateLoginWhenUserPasswordIsEmpty_ExpectThrowInvalidArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new User("name", "lastname", "email@email.com", string.Empty));
        }

        [Fact]
        public void ValidateLoginWhenUserWhenProvidedPasswordIsEmpty_ExpectIncorrectUsernameOrPassword()
        {
            var user = new User("name", "username", "email", "password");

            var currentResult = AuthenticationBusiness.ValidateLogin(user, string.Empty);

            Assert.True(currentResult.IncorrectUsernameOrPassword);
            Assert.False(currentResult.IsApproved);
        }

        [Theory]
        [InlineData("1234", "1234")]
        [InlineData(" 1234", " 1234")]
        [InlineData("1234 ", "1234 ")]
        [InlineData("abc 123", "abc 123")]
        [InlineData("abc@123", "abc@123")]
        [InlineData("@abc123", "@abc123")]
        [InlineData("abced", "abced")]
        public void ValidateLoginWhenUserWhenPasswordIsValid_ExpectIsLogginApproved(string expectedPassword, string mockedPassword)
        {
            var user = new User("name", "username", "email", expectedPassword);

            var currentResult = AuthenticationBusiness.ValidateLogin(user, mockedPassword);

            Assert.True(currentResult.IsApproved);
        }
    }
}
