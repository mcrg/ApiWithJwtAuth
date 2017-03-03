namespace GatewayWithTokenAuth.Controllers
{
    using System;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Configuration;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly TokenProviderSettings settings;

        public LoginController(TokenProviderSettings settings)
        {
            this.settings = settings;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginSpecification specification)
        {
            var isAuthenticated = IsAuthenticated(specification);
            if (isAuthenticated == false)
            {
                return Unauthorized();
            }

            var token = GetToken(specification);
            return Ok(token);
        }

        private string GetToken(LoginSpecification specification)
        {
            var token = GetTokenPayload(specification);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedJwt;
        }

        private JwtSecurityToken GetTokenPayload(LoginSpecification specification)
        {
            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
                             {
                                 new Claim(JwtRegisteredClaimNames.Sub, specification.Login),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                 new Claim(
                                     JwtRegisteredClaimNames.Iat,
                                     now.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
                                     ClaimValueTypes.Integer64)
                             };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(settings.Expiration),
                signingCredentials: settings.SigningCredentials);
            return jwt;
        }

        private bool IsAuthenticated(LoginSpecification specification) => UserCollection.Users.Contains(specification);
    }

    public static class DateTimeExtensions
    {
        public static double ToUnixTimeSeconds(this DateTime date)
        {
            var dateTime = new DateTime(2015, 05, 24, 10, 2, 0, DateTimeKind.Local);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;
            return unixDateTime;
        }
    }
}
