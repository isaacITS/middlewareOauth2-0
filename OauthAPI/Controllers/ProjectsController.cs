using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OauthAPI.Models.Entities;
using OauthAPI.Tools;

namespace OauthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(DbHelper.GetAllProjects());
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(DbHelper.GetProjectById(id));
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpPost] /*<ENDPOINT REGISTER NEW PROJECT (ADMIN PORTAL)>*/
        public IActionResult Register(Proyecto project)
        {
            try
            {
                if (DbHelper.RegisterProject(project)) return Ok(new { ok = true, msg = $"Se ha registrado el proyecto {project.Nombre}" });
                return Unauthorized(new { ok = false, msg = $"El proyecto {project.Nombre} ya está registrado" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult Update(Proyecto project)
        {
            try
            {
                if (DbHelper.UpdateProject(project)) return Ok(new { ok = true, msg = $"Se ha actualizado el proyecto {project.Nombre}" });
                return Unauthorized(new { ok = false, msg = "No se puede actualizar este proyecto" });
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
                if (DbHelper.UpdateProjectStatus(id) != null) return Ok(new { ok = true, msg = "Estatus del proyecto actualizado" });
                return Unauthorized(new { ok = false, msg = "No se encontró el proyecto" });
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
                if (DbHelper.DeleteProject(id)) return Ok(new { ok = true, msg = "El proyecto ha sido eliminado" });
                return Unauthorized(new { ok = false, msg = "No se encontró el proyecto" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }
    }
}
