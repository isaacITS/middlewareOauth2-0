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

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DbHelper.GetAllUsers());
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(DbHelper.GetUserById(id));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetByEmail(string email)
        {
            try
            {
                return Ok(DbHelper.GetUserByEmail(email));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }


        [HttpPost] /*<ENDPOINT REGISTER NEW USER (ADMIN PORTAL)>*/
        public IActionResult Register(Usuario user)
        {
            try
            {
                if (DbHelper.RegisterUser(user)) return Ok(new { ok = true, msg = $"Se ha registrado el usuario {user.Nombre}" });
                return Unauthorized(new { ok = false, msg = $"El correo {user.Email} ya está registrado" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult Update(Usuario user)
        {
            try
            {
                if (DbHelper.UpdateUser(user)) return Ok(new { ok = true, msg = $"Se ha actualizado el usuario {user.Nombre}" });
                return Unauthorized(new { ok = false, msg = "No se pudó encontrar el usaurio" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                if (DbHelper.UpdateUserStatus(id) != null) return Ok(new { ok = true, msg = "Estatus de usuario actualizado" });
                return Unauthorized(new { ok = false, msg = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                if (DbHelper.DeleteUser(id)) return Ok(new { ok = true, msg = "Usuario eliminado con éxito" });
                return Unauthorized(new { ok = false, msg = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }
    }
}
