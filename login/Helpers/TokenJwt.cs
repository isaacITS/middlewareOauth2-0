namespace login.Helpers
{
    public class TokenJwt
    {
        public string CreateToken(string projectName, string imageUrl, string email)
        {
            var expiresDate = DateTime.Now.AddMinutes(15);
            return (Encrypt.EncryptString($"{ expiresDate }${projectName}${imageUrl}${email}"));
        }

        public bool TokenIsValid(string token)
        {
            DateTime dateTimeNow = DateTime.Now;
            string[] result =  Encrypt.DecryptString(token).Split("$");
            DateTime dateTimeExpires = DateTime.Parse(result[0]);
            if(dateTimeExpires < dateTimeNow) return false;
            return true;
        }
    }
}
