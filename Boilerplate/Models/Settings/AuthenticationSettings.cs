namespace Application.Models.Settings
{
    public class AuthenticationSettings
    {
        public string SecretKey { get; set; }
        public string ClaimsNamespace { get; set; }
        public int TokenLifeTime { get; set; }
        public Roles Roles { get; set; }
    }
}
