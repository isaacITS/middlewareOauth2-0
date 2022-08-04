namespace ITS_Middleware.Tools
{
    public class TokenJwt
    {
        public string CreateToken(int id)
        {
            try
            {
                var expiresDate = DateTime.Now.AddMinutes(15);
                return (Encrypt.EncryptString($"{id}${expiresDate}"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TokenIsValid(string token)
        {
            try
            {
                DateTime dateTimeNow = DateTime.Now;
                string[] result = Encrypt.DecryptString(token).Split("$");
                DateTime dateTimeExpires = DateTime.Parse(result[1]);
                if (dateTimeExpires < dateTimeNow)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
