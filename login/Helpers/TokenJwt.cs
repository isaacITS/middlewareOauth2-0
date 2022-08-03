namespace login.Helpers
{
    public class TokenJwt
    {
        public string CreateToken(int id)
        {
            var expiresDate = DateTime.Now.AddMinutes(15);
            return (Encrypt.EncryptString($"{ id }${ expiresDate }"));
        }

        public bool TokenIsValid(string token)
        {
            DateTime dateTimeNow = DateTime.Now;
            string[] result =  Encrypt.DecryptString(token).Split("$");
            DateTime dateTimeExpires = DateTime.Parse(result[1]);
            if(dateTimeExpires < dateTimeNow)
            {
                return false;
            }
            return true;
        }
    }
}
