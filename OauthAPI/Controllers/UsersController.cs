using Microsoft.AspNetCore.Mvc;
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


        [HttpPost] /*<ENDPOINT REGISTER NEW USER (ADMIN PORTAL)>*/
        public IActionResult Register(Usuario user)
        {
            try
            {
                user.FechaAlta = DateTime.Now;
                user.Activo = true;
                user.Pass = Encrypt.sha256(user.Pass);
                if (ModelState.IsValid)
                {
                    var registerUser = DbHelper.RegisterUser(user);
                    if (registerUser)
                    {
                        return Ok(new { msg = $"Se ha registrado el usuario {user.Nombre}" });
                    }
                    return Unauthorized(new { msg = $"El correo {user.Nombre} ya está registrado" });
                }
                return Unauthorized(new { msg = "Model is invalid"});
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult Update(Usuario user)
        {
            try
            {
                DbHelper.UpdateUser(user);
                return Ok(new { msg = $"Se ha actualizado el usuario {user.Nombre}" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new {msg = ex.Message.ToString()});
            }
        }

        [HttpPut]
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                var userUpdateResult = DbHelper.UpdateUserStatus(id);
                if (userUpdateResult != 0) return Ok(new { msg = "Estatus de usuario actualizado" });
                return Unauthorized(new { msg = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if (DbHelper.DeleteUser(id)) return Ok(new { msg = "Usuario eliminado con éxito" });
                return Unauthorized(new { msg = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }
    }
}
