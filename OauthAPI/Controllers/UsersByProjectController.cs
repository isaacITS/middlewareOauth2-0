using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OauthAPI.ExceptionsHandler;
using OauthAPI.Helpers;
using OauthAPI.Models.Entities;
using OauthAPI.Tools;

namespace OauthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersByProjectController : ControllerBase
    {
        private readonly ILogger<UsersByProjectController> _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration config;

        public UsersByProjectController(ILogger<UsersByProjectController> logger, IHostEnvironment env, IConfiguration config)
        {
            _logger = logger;
            _env = env;
            this.config = config;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DbHelper.GetAllUsersByProject());
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(DbHelper.GetUserByProjectById(id));
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                return Ok(DbHelper.GetUserByProjectByEmail(email));
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }


        [HttpPost] /*<ENDPOINT REGISTER NEW USER BY PROJECCT>*/
        [Authorize]
        public IActionResult Register(UsuariosProyecto userModel)
        {
            try
            {
                if (DbHelper.RegisterUSerByProject(userModel)) return Ok(new { ok = true, status = 200, msgHeader = "¡Usuario registrado!", msg = $"Se ha registrado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se registró el usuario", msg = $"El correo o teléfono ya está registrado, verificalos e intenta uno nuevo" });
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Update(UsuariosProyecto userModel)
        {
            try
            {
                var oldUser = DbHelper.GetUserByProjectById(userModel.Id);

                if (!DbHelper.isUserProjectEmailAvailable(userModel.Email) && userModel.Email != oldUser.Email) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se actualizó el usuario", msg = $"El correo {userModel.Email} ya está registrado, intenta usar otro" });
                if ((!string.IsNullOrEmpty(userModel.Telefono) && !DbHelper.isUserProjectPhoneAvailable(userModel.Telefono)) && userModel.Telefono != oldUser.Telefono) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se actualizó el usuario", msg = $"El teléfono {userModel.Telefono} ya está registrado, intenta usar otro" });
                if (DbHelper.UpdateUserByProject(userModel)) return Ok(new { ok = true, status = 200, msgHeader = "Usuario actualizado", msg = $"Se ha actualizado el usuario {userModel.NombreCompleto}" });
                throw new Exception();
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }


        [HttpPut]
        public IActionResult UpdateToken(string token, string email, string Url)
        {
            try
            {
                if (!ValidateTokenEncrypted.TokenIsValid(token)) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo enviar el correo", msg = "La información recibida no es válida" });
                var user = DbHelper.GetUserByProjectByEmail(email);
                if (user == null || !user.Activo) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo enviar el correo", msg = "El usuario no se encuentra registrado o se encuentra deshabilitado" });
                var project = DbHelper.GetProjectById(user.IdProyecto == null ? default : user.IdProyecto.Value);
                user.TokenRecovery = token;
                if (!DbHelper.UpdateUserByProject(user)) throw new Exception();
                var tokenJwt = JwtToken.GenerateTokenUpdatePass();
                var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"EmailViewTemplates\UserProject\restorePassword.html"))
                        .Replace("{contact-name}", user.NombreCompleto)
                        .Replace("{project-name}", project.Nombre)
                        .Replace("{image-project-url}", string.IsNullOrEmpty(project.ImageUrl) ? "NotFound" : project.ImageUrl)
                        .Replace("{link-update-pass}", $"{Url}?token={token}&jwt={tokenJwt}");
                SendEmail.SendEmailReq(email, bodyEmail,
                    $"{project.Nombre} - actualización de contraseña",
                    config.GetSection("ApplicationSettings:EmailConfig:email").Value.ToString(),
                    config.GetSection("ApplicationSettings:EmailConfig:password").Value.ToString());
                return Ok(new { ok = true, status = 200, msgHeader = "Se ha enviado el correo electrónico", msg = $"Hola {user.NombreCompleto} te hemos enviado un correo electrónico para actualizar tu conttraseña" });
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdatePassword(UpdateData updateData)
        {
            try
            {
                if (!ValidateTokenEncrypted.TokenIsValid(updateData.Token)) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo actualizar la contraseña", msg = "El link para actualizar la contraseña no es válido" });
                var user = DbHelper.GetUserByProjectByEmail(updateData.Email);
                if (user.TokenRecovery != updateData.Token) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo actualizar la contraseña", msg = "El link para actualizar la contraseña no es válido" });
                user.TokenRecovery = null;
                user.Pass = updateData.NewPass;
                if (DbHelper.UpdateUserByProject(user)) return Ok(new { ok = true, status = 200, msgHeader = "Contraseña actualizada con exito", msg = $"Hola {user.NombreCompleto} ahora puedes ingresar con tu nueva contraseña" });
                throw new Exception();
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                var user = DbHelper.GetUserByProjectById(id);
                string action = user.Activo ? "desactivado" : "activado";
                if (DbHelper.UpdateUserByProjectStatus(id) != null) return Ok(new { ok = true, status = 200, msgHeader = "Estatus de usuario actualizado", msg = $"Se ha {action} el usuario {user.NombreCompleto}" });
                return Unauthorized(new { ok = false, status = 400, msg = "No se ha podido encontrar el usuario, intenta más tarde", msgHeader = "No se encontró el usuario" });
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpDelete]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = DbHelper.GetUserByProjectById(id);
                if (DbHelper.DeleteUserByProject(id)) return Ok(new { ok = true, status = 200, msgHeader = "Usuario eliminado con éxito", msg = $"Se ha eliminado el usuario {user.NombreCompleto} permanentemente" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se encontró al usuario", msg = "No ha sido posible encontrar el usuario a elimnar, intenta más tarde" });
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }
    }
}
