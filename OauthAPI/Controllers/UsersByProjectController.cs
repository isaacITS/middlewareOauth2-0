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

        public UsersByProjectController(ILogger<UsersByProjectController> logger)
        {
            _logger = logger;
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
                if (DbHelper.RegisterUSerByProject(userModel)) return Ok(new { ok = true, status = 200, msgHeader = "¡Usaurio registrado!", msg = $"Se ha registrado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se registró el usuario", msg = $"El correo {userModel.Email} ya está registrado, intenta usar otro" });
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
                var oldEmail = DbHelper.GetUserByProjectById(userModel.Id).Email;
                if (!DbHelper.isUserProjectEmailAvailable(userModel.Email) && userModel.Email != oldEmail) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se actualizó el usuario", msg = $"El correo {userModel.Email} ya está registrado, intenta usar otro" });
                if (DbHelper.UpdateUserByProject(userModel)) return Ok(new { ok = true, status = 200, msgHeader = "Usuario actualizado", msg = $"Se ha actualizado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo actualizar el usuario",  msg = "Intenta actualizar el usaurio nuevamente, más tarde"});
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
        public IActionResult UpdateToken(string token, int id)
        {
            try
            {
                if (!ValidateTokenEncrypted.TokenIsValid(token)) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo actualizar el usuario", msg = "La información recibida no es válida" });
                var user = DbHelper.GetUserByProjectById(id);
                user.TokenRecovery = token;
                if (DbHelper.UpdateUserByProject(user)) return Ok(new { ok = true, status = 200, msgHeader = "Correo con token enviado", msg = $"Hola {user.NombreCompleto} te hemos enviado un correo electrónico para actualizar tu conttraseña" });
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
        public IActionResult UpdatePassword(UpdateData updateData)
        {
            try
            {
                if (!ValidateTokenEncrypted.TokenIsValid(updateData.Token)) return Unauthorized(new { ok = false, status = 400, msgHeader = "No se pudo actualizar el usuario", msg = "La información recibida no es válida" });
                var user = DbHelper.GetUserByProjectById(updateData.Id);
                
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
