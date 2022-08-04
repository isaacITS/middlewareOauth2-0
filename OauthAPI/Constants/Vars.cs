using Microsoft.IdentityModel.Tokens;

namespace OauthAPI.Constants
{
    public class Vars
    {
        #region WEB APPLICATION CONFIGURATION

        public static string? CONNECTION_STRING;
        public static SymmetricSecurityKey SECURITY_SECRET_KEY;

        public static string? AUDIENCE;
        public static string? ISSUER;

        #endregion WEB APPLICATION CONFIGURATION
    }
}
