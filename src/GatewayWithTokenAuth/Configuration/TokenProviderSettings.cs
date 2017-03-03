namespace GatewayWithTokenAuth.Configuration
{
    using System;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class TokenProviderSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }

        public TimeSpan Expiration { get; set; } = TimeSpan.FromSeconds(20);

        public SigningCredentials SigningCredentials => new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)), SecurityAlgorithms.HmacSha256);
    }
}
