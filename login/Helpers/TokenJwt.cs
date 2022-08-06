namespace login.Helpers
{
    public class TokenJwt
    {
        public string CreateToken(int id, string projectName, string imageUrl)
        {
            var expiresDate = DateTime.Now.AddMinutes(15);
            return (Encrypt.EncryptString($"{ id }${ expiresDate }${projectName}${imageUrl}"));
        }

        public bool TokenIsValid(string token)
        {
            DateTime dateTimeNow = DateTime.Now;
            string[] result =  Encrypt.DecryptString(token).Split("$");
            DateTime dateTimeExpires = DateTime.Parse(result[1]);
            if(dateTimeExpires < dateTimeNow) return false;
            return true;
        }
    }
}
