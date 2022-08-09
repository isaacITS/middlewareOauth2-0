using Microsoft.IdentityModel.Tokens;
using OauthAPI.Constants;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace OauthAPI.Helpers
{
    public class JwtToken
    {

        public static string GenerateToken()
        {
            try
            {
                var credentials = new SigningCredentials(Vars.SECURITY_SECRET_KEY, SecurityAlgorithms.HmacSha256);
                var headers = new JwtHeader(credentials);

                DateTime expiresDate = DateTime.UtcNow.AddHours(2);
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
            catch (Exception)
            {
                throw;
            }
        }

        public static string GenerateTokenUpdatePass()
        {
            try
            {
                var credentials = new SigningCredentials(Vars.SECURITY_SECRET_KEY, SecurityAlgorithms.HmacSha256);
                var headers = new JwtHeader(credentials);

                DateTime expiresDate = DateTime.UtcNow.AddMinutes(15);
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
            catch (Exception)
            {
                throw;
            }
        }

        public static string NewTokenForSignIn(string name, string email, string phone)
        {
            try
            {
                var credentials = new SigningCredentials(Vars.SECURITY_SECRET_KEY, SecurityAlgorithms.HmacSha256);
                var headers = new JwtHeader(credentials);
                DateTime expiresDate = DateTime.UtcNow.AddHours(24);
                int ts = (int)(expiresDate - new DateTime(1970, 1, 1)).TotalSeconds;
                var payload = new JwtPayload
            {
                {"nom", name },
                {"ema", email },
                {"tel", phone },
                {"exp", ts },
                {"iss", Vars.ISSUER },
                {"aud", Vars.AUDIENCE }
            };

                var secToken = new JwtSecurityToken(headers, payload);
                var handler = new JwtSecurityTokenHandler();
                var stringToken = handler.WriteToken(secToken);
                return stringToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
