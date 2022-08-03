using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DbHelper.GetAllUsersByProject());
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(DbHelper.GetUserByProjectById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public IActionResult GetByEmail([FromBody] string email)
        {
            try
            {
                return Ok(DbHelper.GetUserByProjectByEmail(email));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }


        [HttpPost] /*<ENDPOINT REGISTER NEW USER BY PROJECCT>*/
        public IActionResult Register(UsuariosProyecto userModel)
        {
            try
            {
                if (DbHelper.RegisterUSerByProject(userModel)) return Ok(new { ok = true, status = 200, msgHeader = "¡Usaurio registrado!", msg = $"Se ha registrado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se registró el usuario", msg = $"El correo {userModel.Email} ya está registrado, intenta usar otro" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpDelete]
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
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }
    }
}
