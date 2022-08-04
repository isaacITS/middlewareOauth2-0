using Microsoft.IdentityModel.Tokens;
using OauthAPI.Constants;
using System.IdentityModel.Tokens.Jwt;

namespace OauthAPI.Helpers
{
    public class JwtToken
    {

        public static string GenerateToken()
        {
            var credentials = new SigningCredentials(Vars.SECURITY_SECRET_KEY, SecurityAlgorithms.HmacSha256);
            var headers = new JwtHeader(credentials);

            DateTime expiresDate = DateTime.UtcNow.AddHours(3);
            int ts = (int)(expiresDate - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new JwtPayload
            {
                {"exp", ts },
                {"iss", Vars.ISSUER },
                {"aud", Vars.AUDIENCE }
            };

            var secToken = new JwtSecurityToken(headers, payload);

            var handler = new JwtSecurityTokenHandler();

            var stringToken = handler.WriteToken(secToken);

            return stringToken;
        }
    }
}
