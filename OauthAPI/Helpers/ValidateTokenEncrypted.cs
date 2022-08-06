using OauthAPI.Tools;

namespace OauthAPI.Helpers
{
    public class ValidateTokenEncrypted
    {
        public static bool TokenIsValid(string token)
        {
            DateTime dateTimeNow = DateTime.Now;
            string[] result = Encrypt.DecryptString(token).Split("$");
            DateTime dateTimeExpires = DateTime.Parse(result[1]);
            var userObtained = DbHelper.GetUserByProjectById(int.Parse(result[0]));
            if (dateTimeExpires < dateTimeNow || userObtained == null) return false;
            return true;
        }
    }
}
