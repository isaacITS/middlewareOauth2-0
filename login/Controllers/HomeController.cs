using login.ExceptionsHandler;
using login.Helpers;
using login.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly RequestHelper requestHelper = new();
        private readonly IHostEnvironment _env;
        readonly TokenJwt tokenJwt = new();
        private readonly IConfiguration config;

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
                ViewData["ProjectName"] = resultProject.Nombre;
                if (resultProject.Activo) return View();
                return View("NotFound");
            }
            catch (Exception ex)
            {
                List<string> errors = new();
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
                List<string> errors = new();
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
        public async Task<IActionResult> SendEmailRes(string email, string projectName, string projectImage)
        {
            try
            {
                var user = await requestHelper.GetUserProject(email);
                if (user != null && !string.IsNullOrEmpty(user.NombreCompleto))
                {
                    var token = tokenJwt.CreateToken(user.Id, projectName, string.IsNullOrEmpty(projectImage) ? "NoImage" : projectImage);
                    var response = await requestHelper.UpdateToken(token, user.Id);
                    if (!response.Ok) return Json(response);
                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlTemplates\restorePassword.html"))
                        .Replace("{contact-name}", user.NombreCompleto)
                        .Replace("{project-name}", projectName)
                        .Replace("{image-project-url}", string.IsNullOrEmpty(projectImage) ? "NotFound" : projectImage)
                        .Replace("{link-update-pass}", $"{baseUrl}/Home/UpdatePass?token={token}");
                    SendEmail.SendEmailReq(email, bodyEmail, 
                        $"{projectName} - actualización de contraseña", 
                        config.GetSection("ApplicationSettings:EmailConfig:email").Value.ToString(),
                        config.GetSection("ApplicationSettings:EmailConfig:password").Value.ToString());
                    return Json(response);
                }
                return Json(new { ok = false, status = 400, msg = $"Al parecer no existe un usuario registrado con correo: {email}", msgHeader = "No se pudo enviar el correo" });
            }
            catch (Exception ex)
            {
                List<string> errors = new();
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

        public IActionResult UpdatePassword(string token)
        {
            try
            {
                ViewBag.Message = "El token para actualizar la contraseña no es válido o ha expirado";
                if (string.IsNullOrEmpty(token) || !tokenJwt.TokenIsValid(token)) return View("NotFound");

                var dataUpdate = new UpdateData
                {
                    Token = token,
                    Id = int.Parse(Encrypt.DecryptString(token).Split("$")[0]),
                    ImageProject = Encrypt.DecryptString(token).Split("$")[2],
                    ProjectName = Encrypt.DecryptString(token).Split("$")[1]
                };
                return View(dataUpdate);
            }
            catch (Exception ex)
            {
                List<string> errors = new();
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
        public async Task<IActionResult> UpdatePassword(UpdateData updateData)
        {
            try
            {
                ViewBag.Message = "El enlace para actualizar la contraseña ha expirado o no es válido";
                if (string.IsNullOrEmpty(updateData.NewPass) || !tokenJwt.TokenIsValid(updateData.Token)) return View("NotFound");
                updateData.NewPass = Encrypt.Sha256(updateData.NewPass);
                var response = await requestHelper.UpdatePasword(updateData);
                return Json(response);
            }
            catch (Exception ex)
            {
                List<string> errors = new();
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
