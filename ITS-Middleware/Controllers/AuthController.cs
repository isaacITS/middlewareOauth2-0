using ITS_Middleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.Services;

namespace ITS_Middleware.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public MiddlewareDbContext _context;

        public AuthController(MiddlewareDbContext master, ILogger<AuthController> logger)
        {
            _context = master;
            _logger = logger;
        }


        public IActionResult Login()
        {
            try
            {
                var adminEmail = "admin.its@seekers.com";
                var checkAdmin = _context.Usuarios.FirstOrDefault(u => u.Email == adminEmail);
                if (checkAdmin != null)
                {
                    Console.WriteLine("Usuario Principal y ha sido registrado");
                }
                else
                {
                    Usuario usuario = new()
                    {
                        Activo = true,
                        Nombre = "Administrador",
                        FechaAlta = DateTime.Now,
                        Email = adminEmail,
                        Pass = Tools.Encrypt.GetSHA256("admin123"),
                        Puesto = "Administracion"
                    };
                    _context.Add(usuario);
                    _context.SaveChanges();
                    Console.WriteLine("Usaurio principal registrado");
                }

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return View();
                }
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return RedirectToAction("Projects", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }



        /*      Methods Requests        */
        //Autenticacion de credenciales
        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                var user = _context.Usuarios.Where(foundUser => foundUser.Email == email);
                if (user.Any())
                {
                    if (user.Where(s => s.Email == email && s.Pass == Encrypt.GetSHA256(pass)).Any())
                    {
                        HttpContext.Session.SetString("userEmail", email);
                        ViewBag.msg = null;
                        ViewBag.alertType = null;
                        return RedirectToAction("Projects", "Home");
                    }
                    else
                    {
                        ViewBag.msg = "Contraseña Incorrecta";
                        return View("Login");
                    }
                }
                ViewBag.msg = "Usuario no registrado";
                return View("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());

                return Json("Error");
            }

        }




        //Clear session
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userEmail");
                return View("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }
    }
}