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

        public async Task<IActionResult> SignIn(string project)
        {
            ErrorViewModel errorModel = new();
            ViewData["ProjectName"] = project;
            errorModel.ErrorCore = 404; errorModel.ErrorMessageHeader = "No se ingresó algún proyecto para inicio de sesión"; errorModel.ErrorMessage = "No se encontró un proyecto registrado con el nombre ingresado";
            if (project != null)
            {
                var resultProject = await requestHelper.GetProjectByName(project);
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
        public async Task<IActionResult> SignIn(string email, string pass, string phoneNumber)
        {
            try
            {
                var response = await requestHelper.SignIn(email, pass, phoneNumber);
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


        public IActionResult RestorePass()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, status = 500, msg = ex });
            }
        }        
    }
}
