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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DbHelper.GetAllUsers());
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
                return Ok(DbHelper.GetUserById(id));
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
                return Ok(DbHelper.GetUserByEmail(email));
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


        [HttpPost] /*<ENDPOINT REGISTER NEW USER (ADMIN PORTAL)>*/
        [Authorize]
        public IActionResult Register(Usuario user)
        {
            try
            {
                if (DbHelper.RegisterUser(user)) return Ok(new { ok = true, msg = $"Se ha registrado el usuario {user.Nombre}", msgHeader = "Usuario registrado", status = 200 });
                return Unauthorized(new { ok = false, msg = $"El correo {user.Email} ya está registrado", msgHeader = "No se registró el usuario", status = 400 });
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
        public IActionResult Update(Usuario user)
        {
            try
            {
                var oldEmail = DbHelper.GetUserById(user.Id).Email;
                if (!DbHelper.isEmailAvailable(user.Email) && user.Email != oldEmail) return Unauthorized(new { ok = false, msg = $"El correo {user.Email} ya se encuentra registrado, intenta con otro", msgHeader = "Usuario no actualizado", status = 400 });
                if (DbHelper.UpdateUser(user)) return Ok(new { ok = true, msg = $"Se ha actualizado el usuario {user.Nombre}", msgHeader = "Usuario actualizado", status = 200 });
                return Unauthorized(new { ok = false, msg = "No se pudo encontrar el usuario", msgHeader = "Usuario no encontrado", status = 400 });
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
                var user = DbHelper.GetUserById(id);
                string message = user.Activo ? "desactivado" : "activado";
                if (DbHelper.UpdateUserStatus(id) != null) return Ok(new { ok = true, msgHeader = "Estatus de usuario actualizado", msg = $"Se ha {message} el usuario {user.Nombre}", status = 200 });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se activó/desactivó el usuario", msg = "No se es posible activar o desactivar el usuario" });
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
                var user = DbHelper.GetUserById(id);
                if (DbHelper.DeleteUser(id)) return Ok(new { ok = true, status = 200, msgheader = "Usuario eliminado con éxito", msg = $"Se ha eliminado el usaurio {user.Nombre} de forma permanente" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se eliminó el usuario", msg = "No es posible eliminar el usaurio" });
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
