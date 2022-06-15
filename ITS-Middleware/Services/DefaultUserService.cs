using ITS_Middleware.Models.Context;

namespace ITS_Middleware.Services
{
    public class DefaultUserService
    {
        public middlewareITSContext? _context;

        public DefaultUserService(middlewareITSContext context)
        {
            _context = context;
            try
            {
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
            }
        }
    }

        
}
