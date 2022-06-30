using System.Security.Cryptography;
using System.Text;

namespace OauthAPI.Tools
{
    public class Encrypt
    {
        public static string sha256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            var hash = new StringBuilder();
            byte[] crypto = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
