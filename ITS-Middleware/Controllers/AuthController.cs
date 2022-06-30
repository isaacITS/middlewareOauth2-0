using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Models.Context;
using System.Net.Mail;
using System.Net;

namespace ITS_Middleware.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public middlewareITSContext _context;
        private readonly IHostEnvironment _env;

        public AuthController(middlewareITSContext master, ILogger<AuthController> logger, IHostEnvironment env)
        {
            _context = master;
            _logger = logger;
            _env = env;
        }


        public IActionResult Login()
        {
            try
            {
                var adminEmail = "admin.its@seekers.com";
                var checkAdmin = _context.Usuarios.FirstOrDefault(u => u.Email == adminEmail);
                if (checkAdmin == null)
                {
                    Usuario usuario = new()
                    {
                        Activo = true,
                        Nombre = "Administrador",
                        FechaAlta = DateTime.Now,
                        Email = adminEmail,
                        Pass = Encrypt.sha256("admin123"),
                        Puesto = "Administrador"
                    };
                    _context.Add(usuario);
                    _context.SaveChanges();
                }
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return View();
                }
                ViewBag.userName = HttpContext.Session.GetString("userName");
                return RedirectToAction("Projects", "Home");
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
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
                if (user.Pass != Encrypt.sha256(pass))
                {
                    ViewBag.msg = "Contraseña Incorrecta";
                    return View();
                }

                HttpContext.Session.SetString("userName", user.Nombre);
                HttpContext.Session.SetString("idUser", user.Id.ToString());
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }


        public IActionResult RestorePass()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }

        //Restore password method
        [HttpPost]
        public IActionResult GenerateToken([FromBody] string email)
        {
            try
            {
                var token = Encrypt.sha256(email);
                var user = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();

                if (user != null)
                {
                    if (!user.Activo) return Json(new { ok = false, status = 205, msg = "El usuario se encuentra deshabilitado" });
                    user.TokenRecovery = token;
                    var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Email.Equals(user.Email));
                    if (local != null) _context.Entry(local).State = EntityState.Detached;
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\resetPassMessage.html"))
                        .Replace("{contact-name}", user.Nombre)
                        .Replace("{link-update-pass}", $"{baseUrl}/Auth/UpdatePass?token={token}");
                    SendEmail(email, bodyEmail, "Recuperación de contraseña Portal de Configuración");
                    return Json(new { ok = true, status = 200, msg = $"Se ha enviado un correo a {email} para restablecer la contraseña" });
                }
                return Json(new { ok = false, status = 400, msg = $"No se encontró el usuario con correo {email}" });
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }

        public IActionResult UpdatePass(string token)
        {
            try
            {
                var getUser = _context.Usuarios.Where(u => u.TokenRecovery == token).FirstOrDefault();
                if (getUser != null)
                {
                    return View("UpdatePassword", getUser);
                }
                return Json(new { ok = false, status = 400, msg = "El token ha expirado o no es válido" });
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json("Error");
            }
        }

        [HttpPost]
        public IActionResult UpdatePass(Usuario userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(userModel.Id));
                    if (local != null) _context.Entry(local).State = EntityState.Detached;
                    userModel.TokenRecovery = null;
                    userModel.Pass = Encrypt.sha256(userModel.Pass);
                    _context.Entry(userModel).State = EntityState.Modified;
                    _context.SaveChangesAsync();
                    return Json(new { ok = true, status = 200, msg = "Se ha actualizado la contraseña" });
                }
                return Json(new { ok = false, status = 400, msg = "No se recibió un modelo válido" });
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "No se recibió un modelo válido" });
            }
        }



        //Clear session
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userName");
                HttpContext.Session.Remove("idUser");
                return View("Login");
            }
            catch (Exception ex)
            {
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }



        public void SendEmail(string toEmail, string body, string subject)
        {
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("noreply.its.portalconfig@gmail.com", "dvqxxkdwmuynsboa");
            smtpClient.EnableSsl = true;

            var email = new MailMessage();
            email.From = new MailAddress("noreply.its.portalconfig@gmail.com");
            email.To.Add(toEmail);
            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = true;

            smtpClient.Send(email);
        }
    }
}