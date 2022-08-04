using System.Security.Cryptography;
using System.Text;

namespace login.Helpers
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


        //Encriptation for token
        public static string EncryptString(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string DecryptString(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
