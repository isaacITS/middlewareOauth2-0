using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.ExceptionsHandler;
using System.Net.Mail;
using System.Net;
using ITS_Middleware.Helpers;

namespace ITS_Middleware.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        TokenJwt tokenJwt = new();
        RequestHelper requestHelper = new();
        private readonly IHostEnvironment _env;

        public AuthController(ILogger<AuthController> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }


        public IActionResult Login()
        {
            try
            {
                Usuario usuario = new()
                {
                    Activo = true,
                    Nombre = "Administrador",
                    FechaAlta = DateTime.Now,
                    Email = "admin.its@seekers.com",
                    Pass = Encrypt.sha256("admin123"),
                    Puesto = "Administrador"
                };
                var response = requestHelper.RegisterUser(usuario);
                Console.WriteLine(response.Msg);
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
                return View("Error");
            }
        }



        /*      Methods Requests        */
        //Autenticacion de credenciales
        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                var response = requestHelper.SignIn(email, Encrypt.sha256(pass));
                if (response.Ok == true)
                {
                    string id = response.MsgHeader;
                    string nombre = response.Msg;
                    HttpContext.Session.SetString("userName", nombre);
                    HttpContext.Session.SetString("idUser", id);
                    return RedirectToAction("Home", "Home");
                }
                ViewBag.msg = response.Msg;
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
                return View("Error");
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
                return View("Error");
            }
        }

        //Restore password method
        [HttpPost]
        public IActionResult GenerateToken([FromBody] string email)
        {
            try
            {
                var user = requestHelper.GetUserByEmail(email);
                if (user != null)
                {
                    if (!user.Activo) return Json(new { ok = false, status = 205, msg = "El usuario se encuentra deshabilitado" });
                    var token = tokenJwt.CreateToken(user.Id);
                    var response = requestHelper.UpdateUserTokenRecovery(email, token);
                    if (response.Ok)
                    {
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                        var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\resetPassMessage.html"))
                            .Replace("{contact-name}", user.Nombre)
                            .Replace("{link-update-pass}", $"{baseUrl}/Auth/UpdatePass?token={token}");
                        SendEmail(email, bodyEmail, "Recuperación de contraseña, portal de administración");
                        return Json(new { ok = true, status = 200, msg = $"Se ha enviado un correo a {email} para restablecer la contraseña" });
                    }
                    return Json(new { ok = false, status = 400, msg = response.Msg });
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
                return View("Error");
            }
        }

        public IActionResult UpdatePass(string token)
        {
            try
            {
                if (tokenJwt.TokenIsValid(token))
                {
                    string[] dataToken = Encrypt.DecryptString(token).Split("$");
                    int id = int.Parse(dataToken[0]);
                    var user = requestHelper.GetUserById(id);
                    if (user != null)
                    {
                        if (user.TokenRecovery == token) return View("UpdatePassword", user);
                    }
                }
                ViewBag.msg = "El enlace para recuperar contraseña ha expirado o no es válido";
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
                return Json(new { ok = false, status = 500, msg = "Internal Server Error" });
            }
        }

        [HttpPost]
        public IActionResult UpdatePass(Usuario userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userModel.Pass = Encrypt.sha256(userModel.Pass);
                    var response = requestHelper.UpdateUserPassword(userModel);
                    if(response.Ok) return Json(new { ok = true, status = 200, msg = response.Msg, MsgHeader = response.MsgHeader });
                    return Json(new { ok = false, status = 400, msg = response.Msg, MsgHeader = response.MsgHeader });
                }
                return Json(new { ok = false, status = 400, msg = "Los datos recibidos no son válidos o están incompletos", MsgHeader = "Información no válida"});
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