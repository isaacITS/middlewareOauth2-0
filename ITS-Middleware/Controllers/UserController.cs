using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;

namespace ITS_Middleware.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        RequestHelper requestHelper = new();

        public UserController(ILogger<UserController> logger)
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
        public IActionResult Register(Usuario user)
        {
            try
            {
                user.FechaAlta = DateTime.Now;
                user.Activo = true;
                if (ModelState.IsValid)
                {
                    var response = requestHelper.RegisterUser(user);
                    if (!response.Ok)
                    {
                        return Json(new { ok = false, status = 410, msg = response.Msg });
                    }
                    return Json(new { ok = true, status = 200, msg = response.Msg });
                }
                return Json(new { ok = false, status = 410, msg = "Información inválida o incompleta" });
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
        public IActionResult EditUser(int id)
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
                        return Json(new { ok = false, msg = "No se ingresó un ID válido o no puede ser editado" });
                    }
                    var usuario = requestHelper.GetUserById(id);
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
        public IActionResult UpdateUser(Usuario user)
        {
            try
            {
                var response = requestHelper.UpdateUser(user);
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
                    var usuario = requestHelper.GetUserById(id);
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
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                var response = requestHelper.UpdateUserStatus(id);
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
        public IActionResult DeleteUser(int id)
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
                        return Json(new { ok = false, msg = "No se ingresó un ID válido o no puede ser eliminado" });
                    }
                    var usuario = requestHelper.GetUserById(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID No coincide con un usuario registrado" });
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
        public IActionResult DeleteUserPost([FromBody] int id)
        {
            try
            {
                var response = requestHelper.DeleteUser(id);
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
    }
}
