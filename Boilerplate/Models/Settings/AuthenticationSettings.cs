namespace Application.Models.Settings
{
    public record AuthenticationSettings
    {
        public string SecretKey { get; init; }
        public string ClaimsNamespace { get; init; }
        public int TokenLifeTime { get; init; }
        public int MaxAttempts { get; init; }
    }
}
