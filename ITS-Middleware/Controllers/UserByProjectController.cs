using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Helpers;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;

namespace ITS_Middleware.Controllers
{
    public class UserByProjectController : Controller
    {
        private readonly ILogger<UserByProjectController> _logger;
        RequestHelper requestHelper = new();
        private readonly IHostEnvironment _env;

        public UserByProjectController(ILogger<UserByProjectController> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))  return RedirectToAction("Login", "Auth");
                
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
                var password = user.Pass;
                var baseUrl = "https://its-oauth-login.azurewebsites.net/";
                user.FechaCreacion = DateTime.Now;
                user.FechaAcceso = user.FechaCreacion;
                user.Activo = true;

                if (ModelState.IsValid)
                {
                    var response = await requestHelper.RegisterUserByProject(user);
                    if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                    if (!response.Ok) return Json(response);

                    int IdProyect = user.IdProyecto == null ? default : user.IdProyecto.Value;
                    var project = await requestHelper.GetProjectById(IdProyect);

                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\CreacionNuevaCuenta.html"))
                        .Replace("{contact-name}", user.NombreCompleto)
                        .Replace("{nombre-empresa}", project.Nombre)
                        .Replace("{link-update}", baseUrl + $"?project={project.Nombre}")
                        .Replace("{user-email}", user.Email)
                        .Replace("{user-pass}", password)
                        .Replace("{imagen-empresa}", !string.IsNullOrEmpty(project.ImageUrl) ? project.ImageUrl : "¡Bienvenido!");
                    SendEmail.SendEmailReq(user.Email, bodyEmail, $"Nueva cuenta para: {project.Nombre}");
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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    ViewData["ProjectsList"] = await requestHelper.GetAllProjects();
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, Status = 400, Msg = "El ID no coincide con un usuario registrado" });
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
        public async Task<IActionResult> Update(UsuariosProyecto user)
        {
            try
            {
                string password = user.Pass;
                string email = user.Email;
                string phone = user.Telefono;
                var oldUser = await requestHelper.GetUserByProjectById(user.Id);
                var response = await requestHelper.UpdateUserByProject(user);
                var baseUrl = "https://its-oauth-login.azurewebsites.net/";
                if (response.Status == 500 || response.Status == 401) throw new Exception(response.Msg);
                if (response.Ok)
                {
                    if (oldUser.Email != email || (!string.IsNullOrEmpty(password) && oldUser.Pass != Encrypt.sha256(password)) || oldUser.Telefono != phone)
                    {
                        int IdProyect = user.IdProyecto == null ? default : user.IdProyecto.Value;
                        var project = await requestHelper.GetProjectById(IdProyect);

                        string dataUpdated = oldUser.Email != email && oldUser.Pass != user.Pass ? "el correo y contraseña" : oldUser.Email != email ? "el correo" : oldUser.Pass != user.Pass ? "la contraseña" : "";
                        dataUpdated += oldUser.Telefono != phone ? ", tu Teléfono se ha actualizado": "No se agregó un Teléfono.";
                        password = oldUser.Pass != Encrypt.sha256(password) ? password : "Tu contraseña sigue siendo la misma";
                         

                        var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"wwwroot\htmlViews\CambioDatos.html"))
                            .Replace("{contact-name}", user.NombreCompleto)
                            .Replace("{nombre-empresa}", project.Nombre)
                            .Replace("{link-update}", baseUrl + $"?project={project.Nombre}")
                            .Replace("{dato}", dataUpdated)
                            .Replace("{user-email}", email)
                            .Replace("{user-pass}", password)
                            .Replace("{user-phone}", string.IsNullOrEmpty(phone) ? "" : phone)
                            .Replace("{imagen-empresa}", !string.IsNullOrEmpty(project.ImageUrl) ? project.ImageUrl : "¡Bienvenido!");

                        SendEmail.SendEmailReq(oldUser.Email, bodyEmail, $"¡Se han actualizado sus datos de: {project.Nombre}");
                        if (oldUser.Email != email) SendEmail.SendEmailReq(email, bodyEmail, $"¡Se han actualizado sus datos de: {project.Nombre}");
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
                        return Json(new { Ok = false, status = 400, Msg = "No se ingresó un ID válido o no puede ser activado/desactivado" });
                    }
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, status = 400, Msg = "El ID no coincide con un usuario registrado" });
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
        public async Task<IActionResult> UpdateStatus([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.UpdateUserByProjectStatus(id);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName"))) return RedirectToAction("Login", "Auth");
                    var usuario = await requestHelper.GetUserByProjectById(id);
                    if (usuario == null)
                    {
                        return Json(new { Ok = false, Msg = "El ID No coincide con un usuario registrado" });
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
        public async Task<IActionResult> DeletePost([FromBody] int id)
        {
            try
            {
                var response = await requestHelper.DeleteUserByProject(id);
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
