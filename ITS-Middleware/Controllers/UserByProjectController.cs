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

        public async Task<IActionResult> Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var projects = await requestHelper.GetAllProjects();
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsuariosProyecto user)
        {
            try
            {
                user.FechaCreacion = DateTime.Now;
                user.FechaAcceso = user.FechaCreacion;
                user.Activo = true;
                if (ModelState.IsValid)
                {
                    var response = await requestHelper.RegisterUserByProject(user);
                    return Json(response);
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }


        //Editar usuario
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    ViewData["ProjectsList"] = await requestHelper.GetAllProjects();
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "El ID no coincide con un usuario registrado" });
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UsuariosProyecto user)
        {
            try
            {
                var response = await requestHelper.UpdateUserByProject(user);
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


        /*UPDATE USER STATUS*/
        public async Task<IActionResult> ChangeStatus(int id)
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
                        return Json(new { Ok = false, status = 400, Msg = "No se ingresó un ID válido o no puede ser activado/desactivado" });
                    }
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, status = 400, Msg = "El ID no coincide con un usuario registrado" });
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.UpdateUserByProjectStatus(id);
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

        //Eliminar usuario
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, Msg = "El ID No coincide con un usuario registrado" });
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
                return Json(new { Ok = false, Status = 500, Msg = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.DeleteUserByProject(id); 
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
    }
}
