using ITS_Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITS_Middleware.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public middlewareITSContext _context;

        public AuthController(middlewareITSContext master, ILogger<AuthController> logger)
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
                var user = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.msg = "Usuario no registrado";
                    return View();
                }
                if (!user.Activo)
                {
                    ViewBag.msg = "El usuario esta deshabilitado";
                    return View();
                }
                if (user.Pass != Encrypt.GetSHA256(pass))
                {
                    ViewBag.msg = "Contraseña Incorrecta";
                    return View();
                }
                HttpContext.Session.SetString("userEmail", email);
                HttpContext.Session.SetString("idUser", user.Id.ToString());
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }

        }


        public IActionResult RestorePassBefore()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }

        //Restore password method
        [HttpPost]
        public IActionResult RestorePass(string email)
        {
            try
            {
                var token = Encrypt.GetSHA256(Guid.NewGuid().ToString());
                var user = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();

                if (user != null)
                {
                    var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Email.Equals(user.Email));
                    if (local != null) _context.Entry(local).State = EntityState.Detached;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return View("RestorePass", user);
                }
                return Json(new { ok = false, msg = "No se encontró el correo " });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }




        //Clear session
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userEmail");
                HttpContext.Session.Remove("idUser");
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