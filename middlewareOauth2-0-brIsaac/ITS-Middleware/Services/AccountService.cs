using ITS_Middleware.Models;

namespace ITS_Middleware.Services
{
    public interface AccountService
    {
        public User Login(string email, string password);
    }
}
