using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;
using ITS_Middleware.Constants;

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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return View();
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
        public async Task<IActionResult> Login(string email, string pass)
        {
            try
            {
                var response = await requestHelper.SignIn(email, pass);
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                if (response.Ok)
                {
                    string id = response.MsgHeader;
                    string nombre = response.Msg;
                    HttpContext.Session.SetString("userName", nombre);
                    HttpContext.Session.SetString("idUser", id);
                    HttpContext.Session.SetString("SecretToken", response.Token);
                    Vars.SECRET_TOKEN = response.Token;
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
        public async Task<IActionResult> GenerateToken([FromBody] string email)
        {
            try
            {
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}/Auth/UpdatePass";
                var token = tokenJwt.CreateToken(email);
                var response = await requestHelper.UpdateUserTokenRecovery(email, token, baseUrl);
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                return Json(response);
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        public async Task<IActionResult> UpdatePass(string token, string jwt)
        {
            try
            {
                ViewBag.msg = "El enlace para recuperar contraseña ha expirado o no es válido";
                if (!string.IsNullOrEmpty(token) && tokenJwt.TokenIsValid(token))
                {
                    string[] dataToken = Encrypt.DecryptString(token).Split("$");
                    var user = await requestHelper.GetUserByEmail(dataToken[1]);
                    if (user != null)
                    {
                        if (user.TokenRecovery != token) return View("Login");
                        user.TokenRecovery = jwt;
                        return View("UpdatePassword", user);
                    }
                }
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
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePass(Usuario userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await requestHelper.UpdateUserPassword(userModel);
                    if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                    return Json(response);
                }
                return Json(new { Ok = false, Status = 400, Msg = "Los datos recibidos no son válidos o están incompletos", MsgHeader = "Información no válida" });
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }



        //Clear session
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userName");
                HttpContext.Session.Remove("idUser");
                HttpContext.Session.Remove("SecretToken");
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
                return View("Error");
            }
        }
    }
}