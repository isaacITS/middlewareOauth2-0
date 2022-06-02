using ITS_Middleware.Models;
using ITS_Middleware.Models.Entities;

namespace ITS_Middleware.Services
{
    public class DefaultUserService
    {
        public MiddlewareDbContext? _context;

        public void CreateAdmin(MiddlewareDbContext context)
        {
            _context = context;
            try
            {
                var admin = new Usuario
                {
                    Activo = true,
                    Nombre = "Administrador",
                    FechaAlta = new DateTime(),
                    Email = "admin@gmail.com",
                    Pass = Tools.Encrypt.GetSHA256("admin123"),
                    Puesto = "Administracion"
                };

                var checkAdmin = _context.Usuarios.Where(u => u.Email == admin.Email);
                if (checkAdmin.Any())
                {
                    Console.WriteLine("Usuario Principal y ha sido registrado");
                }
                else
                {
                    _context.Usuarios.Add(admin);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
            }
        }
    }

        
}
