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
        private readonly IHostEnvironment _env;

        public UserController(ILogger<UserController> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
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
        public async Task<IActionResult> Register(Usuario user)
        {
            try
            {
                string password = user.Pass;
                user.FechaAlta = DateTime.Now;
                user.Activo = true;
                if (ModelState.IsValid)
                {
                    var response = await requestHelper.RegisterUser(user);
                    if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                    if (!response.Ok)
                    {
                        return Json(response);
                    }
                    var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\newAccount.html"))
                        .Replace("{contact-name}", user.Nombre)
                        .Replace("{link-signin}", baseUrl)
                        .Replace("{user-email}", user.Email)
                        .Replace("{user-pass}", password);
                    SendEmail.SendEmailReq(user.Email, bodyEmail, "Nueva cuenta para Portal de Adminstración Oauth");
                    return Json(response);
                }
                return Json(new { Ok = false, Status = 400, MsgHeader = "Información inválida o incompleta", Msg = "Existen algunos datos que son reuqueridos para el registro" });
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
        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
                    if (id == null || id == 1)
                    {
                        return Json(new { Ok = false, MsgHeader = "No se puede editar el usaurio", Msg = "No se ingresó un ID válido o no puede ser editado" });
                    }
                    var usuario = await requestHelper.GetUserById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, MsgHeader = "No se encontró el usuario", Msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
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
        public async Task<IActionResult> UpdateUser(Usuario user)
        {
            try
            {
                string password = user.Pass;
                string email = user.Email;
                var oldUser = await requestHelper.GetUserById(user.Id);
                var response = await requestHelper.UpdateUser(user);
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                if (response.Ok)
                {
                    if (oldUser.Email != email || (!string.IsNullOrEmpty(password) && oldUser.Pass != Encrypt.sha256(password)))
                    {
                        string dataUpdated = oldUser.Email != email && oldUser.Pass != user.Pass ? "el correo y contraseña" : oldUser.Email != email ? "el correo" : oldUser.Pass != user.Pass ? "la contraseña" : "";
                        password = oldUser.Pass != Encrypt.sha256(password) ? password : "Tu contraseña siguie siendo la misma";
                        var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}";
                        var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\updateAccount.html"))
                            .Replace("{contact-name}", user.Nombre)
                            .Replace("{link-signin}", baseUrl)
                            .Replace("{data-updated}", dataUpdated)
                            .Replace("{user-email}", email)
                            .Replace("{user-pass}", password);
                        SendEmail.SendEmailReq(oldUser.Email, bodyEmail, "Actualización de datos de usuario para Portal de Administración OAuth");
                        if (oldUser.Email != email) SendEmail.SendEmailReq(email, bodyEmail, "Actualización de datos para portal de administración OAuth");
                    }
                    return Json(response);
                }
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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
                    if (id == null || id == 1)
                    {
                        return Json(new { Ok = false, MsgHeader = "No se puede editar el usuario", Msg = "No se ingresó un ID válido o no puede ser activado/desactivado" });
                    }
                    var usuario = await requestHelper.GetUserById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, MsgHeader = "Usuario no encontrado", Msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
                
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
        public async Task<IActionResult> UpdateStatus(int id)
        {
            try
            {
                var response = await requestHelper.UpdateUserStatus(id);
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


        //Eliminar usuario
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
                    if (id == null || id == 1)
                    {
                        return Json(new { Ok = false, MsgHeader = "No se puede encontrar un usuario", Msg = "No se ingresó un ID válido o no puede ser eliminado" });
                    }
                    var usuario = await requestHelper.GetUserById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, MsgHeader = "No se encontró un usuario", Msg = "El ID No coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
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
        public async Task<IActionResult> DeleteUserPost([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.DeleteUser(id);
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
    }
}
