using System.Security.Cryptography;
using System.Text;

namespace ITS_Middleware.Tools
{
    public class Encrypt
    {
        //Encriptar contraseña
        /*
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
        */
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
