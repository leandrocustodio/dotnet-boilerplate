namespace Application.Models.Entities.Authentication
{
    public class LoginResult
    {
        public bool IncorrectUsernameOrPassword { get; set; }
        public bool IsUserBlocked { get; set; }
        public bool IsUserInactive { get; set; }
        public bool IsApproved => (!IsUserBlocked && !IsUserInactive && !IncorrectUsernameOrPassword);
        public User User { get; set; }
    }
}
