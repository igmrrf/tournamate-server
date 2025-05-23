

namespace Infrastructure.InternalServices.Jwt
{
    public class JwtSettings
    {
        public string Key { get; set; } = Environment.GetEnvironmentVariable("JWT_KEY");
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; }
    }
}