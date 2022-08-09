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
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
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
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}/Home/UpdatePassword";
                var token = tokenJwt.CreateToken(projectName, email);
                var response = await requestHelper.UpdateToken(token, email, baseUrl);
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
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

        public async Task<IActionResult> UpdatePassword(string token, string jwt)
        {
            try
            {
                ViewBag.Message = "El enlace para actualizar la contraseña no es válido o ha expirado";
                if (string.IsNullOrEmpty(token) || !tokenJwt.TokenIsValid(token)) return View("NotFound");
                string[] dataToken = Encrypt.DecryptString(token).Split("$");
                var dataUpdate = new UpdateData
                {
                    Email = dataToken[2],
                    Token = token,
                    TokenJwt = jwt,
                    ProjectName = dataToken[1]
                };
                var project = await requestHelper.GetProjectByName(dataUpdate.ProjectName);
                dataUpdate.ImageProject = string.IsNullOrEmpty(project.ImageUrl) ? "NoImg" : project.ImageUrl;
                TempData["userDataUpdate"] = dataUpdate;
                return View();
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
                var response = await requestHelper.UpdatePasword(updateData);
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
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
