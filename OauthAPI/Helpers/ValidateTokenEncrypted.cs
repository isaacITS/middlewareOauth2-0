namespace OauthAPI.Helpers
{
    public class ValidateTokenEncrypted
    {
        public static bool TokenIsValid(string token)
        {
            DateTime dateTimeNow = DateTime.Now;
            string[] result = Encrypt.DecryptString(token).Split("$");
            DateTime dateTimeExpires = DateTime.Parse(result[0]);
            if (dateTimeExpires < dateTimeNow) return false;
            return true;
        }
    }
}
