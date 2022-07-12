using ITS_Middleware.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;

namespace ITS_Middleware.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        RequestHelper requestHelper = new();

        public ProjectController(ILogger<ProjectController> logger)
        {
            _logger = logger;
        }

        //Registrar proyecto
        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var methods = requestHelper.GetAllAuthMethods();
                ViewData["MetodosAuth"] = methods;
                return PartialView();
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


        [HttpPost]
        public IActionResult Register(Proyecto project)
        {
            try
            {
                project.Activo = true;
                project.IdUsuarioRegsitra = int.Parse(HttpContext.Session.GetString("idUser"));
                project.FechaAlta = DateTime.Now;
                if (ModelState.IsValid)
                {
                    var request = requestHelper.RegisterProject(project);
                    if (!request.Ok)
                    {
                        return Json(new { ok = false, msg = $"El proyecto {project.Nombre} ya esta registrado" });
                    }
                    return Json(new { ok = true, msg = $"El proyecto {project.Nombre} se ha registrado" });
                }
                return Json(new { ok = false, msg = "Información incompleta, intenta volver a iniciar sesión"});
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

        //Editar proyecto
        public IActionResult EditProject(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var project = requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un proyecto registrado" });
                    }
                    var metodos = requestHelper.GetAllAuthMethods();
                    ViewData["MetodosAuth"] = metodos;
                    return PartialView(project);
                }
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

        [HttpPost]
        public IActionResult UpdateProject(Proyecto project)
        {
            try
            {
                var response = requestHelper.UpdateProject(project);
                if (response.Ok)
                {
                    return Json(new { ok = true, msg = response.Msg });
                }
                return Json(new { ok = true, msg = response.Msg });
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


        //Eliminar proyecto
        public IActionResult DeleteProject(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var project = requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { ok = false, msg = "No se encontró un proyecto registrado" });
                    }
                    return PartialView(project);
                }
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



        [HttpPost]
        public IActionResult DeleteProjectPost([FromBody] int id)
        {
            try
            {
                var response = requestHelper.DeleteProject(id);
                if (response.Ok)
                {
                    return Json(new { ok = true, msg = response.Msg });
                } 
                return Json(new { ok = false, msg = "No se eliminó el proyecto. No se encontró el proyecto" });
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

        //Estatus activo/desactivo proyectos
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var project = requestHelper.GetProjectById(id);
                    if (project == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView("ChangeStatus", project);
                }
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

        [HttpPost]
        public IActionResult UpdateStatusPost(int id)
        {
            try
            {
                var response = requestHelper.UpdateProjectStatus(id);
                if (response.Ok)
                {
                    return Json(new { ok = true, msg = response.Msg });
                }
                return Json(new { ok = false, msg = response.Msg });
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
    }
}
