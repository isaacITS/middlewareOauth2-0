using login.Helpers;
using login.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;


namespace login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RequestHelper requestHelper = new();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult SignIn(string project)
        {
            ErrorViewModel errorModel = new();
            errorModel.ErrorCore = 404; errorModel.ErrorMessageHeader = "No se ingresó algún proyecto para inicio de sesión"; errorModel.ErrorMessage = "No se encontró un proyecto registrado con el nombre ingresado";
            if (project != null)
            {
                var resultProject = requestHelper.GetProjectByName(project);
                if (resultProject != null)
                {
                    ViewData["project"] = resultProject;
                    if (resultProject.Activo) return View();
                    errorModel.ErrorCore = 403; errorModel.ErrorMessageHeader = "Proyecto deshabilitado"; errorModel.ErrorMessage = $"El proyecto {resultProject.Nombre} se encuentra deshabilidtado";
                }

            }
            ViewData["errorData"] = errorModel;
            return View("Error");
        }

        [HttpPost]
        public IActionResult SignIn(string email, string pass)
        {
            try
            {
                var response = requestHelper.SignIn(email, pass);
                if (response.Ok)
                {
                    return Json(new { ok = false, status = 200, msg = response.Msg, msgHeder = response.MsgHeader });
                }
                return Json(new { ok = true, status = 400, msg = response.Msg, msgHeder = response.MsgHeader });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, status = 500, msg = ex });
            }
        }
    }
}
