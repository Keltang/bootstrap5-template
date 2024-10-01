
namespace BlackBox.Application.Configurations
{
    public class AuthorizationConfig
    {
        public const string ConfigurationName = "Jwt";

        public string? Issuer { get; set; }
        public string Secret { get; set; } = string.Empty;
        public int ExpirationInMinutes { get; set; }
        public int RefreshTokenExpiryInHours { get; set; }
    }
}
 