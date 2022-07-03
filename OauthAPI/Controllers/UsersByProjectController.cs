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
                return Unauthorized(new { msg = ex.Message.ToString() });
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
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }


        [HttpPost] /*<ENDPOINT REGISTER NEW USER BY PROJECCT>*/
        public IActionResult Register(UsuariosProyecto userModel)
        {
            try
            {
                if (DbHelper.RegisterUSerByProject(userModel)) return Ok(new { ok = true, msg = $"Se ha registrado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, msg = $"El correo {userModel.Email} ya está registrado" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult Update(UsuariosProyecto userModel)
        {
            try
            {
                if (DbHelper.UpdateUserByProject(userModel)) return Ok(new { ok = true, msg = $"Se ha actualizado el usuario {userModel.NombreCompleto}" });
                return Unauthorized(new { ok = false, msg = "No se pudó encontrar el usuario" });
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
                if (DbHelper.UpdateUserByProjectStatus(id) != null) return Ok(new { ok = true, msg = "Estatus de usuario actualizado" });
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
                if (DbHelper.DeleteUserByProject(id)) return Ok(new { ok = true, msg = "Se ha eliminado el usuario" });
                return Unauthorized(new { ok = false, msg = "No se encontró al usuario" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }
    }
}
