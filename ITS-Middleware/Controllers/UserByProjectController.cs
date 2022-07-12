using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;
using ITS_Middleware.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ITS_Middleware.Controllers
{
    public class UserByProjectController : Controller
    {
        private readonly ILogger<UserByProjectController> _logger;
        RequestHelper requestHelper = new();

        public UserByProjectController(ILogger<UserByProjectController> logger)
        { 
            _logger = logger;
        }

        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var projects = requestHelper.GetAllProjects();
                ViewData["ProjectsList"] = projects;
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
        public IActionResult Register(UsuariosProyecto user)
        {
            try
            {
                user.FechaCreacion = DateTime.Now;
                user.FechaAcceso = user.FechaCreacion;
                user.Activo = true;
                if (ModelState.IsValid)
                {
                    var response = requestHelper.RegisterUserByProject(user);
                    if (response.Ok)
                    {
                        return Json(new { ok = true, status = 200, msg = response.Msg });
                    }
                    return Json(new { ok = false, status = 410, msg = response.Msg });
                }
                return Json(user);
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


        //Editar usuario
        public IActionResult Update(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = requestHelper.GetUserByProjectById(id);
                    ViewData["ProjectsList"] = requestHelper.GetAllProjects();
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
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
        public IActionResult Update(UsuariosProyecto user)
        {
            try
            {
                var response = requestHelper.UpdateUserByProject(user);
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


        /*UPDATE USER STATUS*/
        public IActionResult ChangeStatus(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null || id == 1)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido o no puede ser activado/desactivado" });
                    }
                    var usuario = requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
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
        public IActionResult UpdateStatus([FromBody] int id)
        {
            try
            {
                var response = requestHelper.UpdateUserByProjectStatus(id);
                if (response.Ok)
                {
                    return Json(new { ok = true, status = 200, msg = response.Msg });
                }
                return Json(new { ok = false, status = 410, msg = response.Msg });
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

        //Eliminar usuario
        public IActionResult Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = true, msg = "El ID No coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
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
        public IActionResult DeletePost([FromBody] int id)
        {
            try
            {
                var response = requestHelper.DeleteUserByProject(id); 
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
