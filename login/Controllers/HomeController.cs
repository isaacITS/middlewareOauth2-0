using login.ExceptionsHandler;
using login.Helpers;
using login.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RequestHelper requestHelper = new();
        private readonly IHostEnvironment _env;
        TokenJwt tokenJwt = new();
        IConfiguration config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IHostEnvironment env)
        {
            _logger = logger;
            this.config = config;
            _env = env;
        }

        public async Task<IActionResult> SignIn(string project)
        {
            try
            {
                if (string.IsNullOrEmpty(project)) return View("NotFound");
                var resultProject = await requestHelper.GetProjectByName(project);

                if (resultProject == null) return View("NotFound");
                ViewData["project"] = resultProject;
                if (resultProject.Activo) return View();
                return View("NotFound");
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
        public async Task<IActionResult> SignIn(SigninData signinData)
        {
            try
            {
                var response = await requestHelper.SignIn(signinData);
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
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> SendEmailRes(string email, string projectName, int projectId)
        {
            try
            {
                var user = await requestHelper.GetUserProject(email);
                if (user != null)
                {
                    var token = tokenJwt.CreateToken(user.Id);
                    user.TokenRecovery = token;
                    var response = await requestHelper.UpdateUserProject(user);
                    if (!response.Ok) return Json(new { ok = false, status = 400, msg = "No se ha enviado el correo, intenta nuevamente", msgHeader = "No se envió el correo" });

                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlTemlates\restorePassword.html"))
                        .Replace("{contact-name}", user.NombreCompleto)
                        .Replace("{nombre-empresa}", projectName)
                        .Replace("{link-update-pass}", $"{baseUrl}/Home/UpdatePass?token={token}");
                    SendEmail.SendEmailReq(email, bodyEmail, $"{projectName} - actualización de contraseña");
                    return Json(response);
                }
                return Json(new { ok = false, status = 400, msg = $"Al parecer no existe un usuario registrado con correo: {email}", msgHeader = "Usuario no encontrado" });
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

        public async Task<IActionResult> UpdatePassword(string token)
        {
            try
            {
                ViewBag.Message = "El token para actualizar la contraseña no es válido";
                if (!string.IsNullOrEmpty(token) && tokenJwt.TokenIsValid(token)) return View("SignIn");

                int id = int.Parse(Encrypt.DecryptString(token).Split("$")[0]);
                var user = await requestHelper.GetUserProjectById(id);
                return View(user);
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
        public async Task<IActionResult> UpdatePassword(UsuariosProyecto userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userModel.Pass = Encrypt.sha256(userModel.Pass);
                    var response = await requestHelper.UpdateUserProject(userModel);
                    return Json(response);
                }
                throw new Exception();
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
