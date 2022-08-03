using FirebaseAdmin.Auth;

namespace OauthAPI.Helpers
{
    public class FirebaseHelper
    {
        public async Task<UserRecord> GetUserByUid(string uid)
        {
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return userRecord;
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebaseAdmin") return null;
                throw;
            }
        }

        public async Task CreateBaseUser(string email, string pass, string name)
        {
            UserRecordArgs args = new()
            {
                Email = email,
                EmailVerified = false,
                Password = pass,
                DisplayName = name,
                Disabled = false,
            };
            try
            {
                UserRecord userRecord = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(args.Email);
            }
            catch (Exception ex)
            {
                if (ex.Source == "FirebaseAdmin") await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
                throw;
            }
        }
    }
}
