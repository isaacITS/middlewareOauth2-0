using ITS_Middleware.Models;

namespace ITS_Middleware.Services
{
    public class AccountServiceImpl : AccountService
    {
        private List<User> account;

        public AccountServiceImpl()
        {
            account = new List<User>
            {
                new User
                {
                    email = "admin@admin.com",
                    password = "admin123"
                }
            };
        }

        public User Login(string email, string password)
        {
            return account.SingleOrDefault(account => account.email == email && account.password == password);
        }
    }
}
