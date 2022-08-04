using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OauthAPI.ExceptionsHandler;
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
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(DbHelper.GetProjectById(id));
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
        public IActionResult GetByName(string project)
        {
            try
            {
                return Ok(DbHelper.GetProjectByName(project));
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

        [HttpPost] /*<ENDPOINT REGISTER NEW PROJECT (ADMIN PORTAL)>*/
        public IActionResult Register(Proyecto project)
        {
            try
            {
                if (DbHelper.RegisterProject(project)) return Ok(new { ok = true, status = 200, msgHeader = "Proyecto registrado con éxito", msg = $"Se ha registrado el proyecto {project.Nombre}" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "No se registró el proyecto", msg = $"El proyecto {project.Nombre} ya está registrado" });
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
        public IActionResult Update(Proyecto project)
        {
            try
            {
                if (DbHelper.UpdateProject(project)) return Ok(new { ok = true, status = 200, msgHeader = "Proyecto actualizado con éxito", msg = $"Se ha actualizado el proyecto {project.Nombre}" });
                return Unauthorized(new { ok = false, msgHeader = "No se pudo encontrar el proyecto", msg = "No se puede actualizar el proyecto, intenta más tarde" });
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
        public IActionResult UpdateStatus(int id)
        {
            try
            {
                var project = DbHelper.GetProjectById(id);
                string action = project.Activo ? "desactivado" : "activado";
                if (DbHelper.UpdateProjectStatus(id) != null) return Ok(new { ok = true, status = 200, msgHeader = "Estatus de proyecto actualizado", msg = $"Se ha {action} el proyecto {project.Nombre}" });
                return Unauthorized(new { ok = false, status = 400, msg = "No se encontró el proyecto, intenta más tarde", msgHeader = "No se ha encontrado el proyecto" });
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
        public IActionResult Delete(int id)
        {
            try
            {
                var project = DbHelper.GetProjectById(id);
                if (DbHelper.DeleteProject(id)) return Ok(new { ok = true, status = 200, msgHeader = "Proyecto eliminado", msg = $"Se ha elimado el proyecto {project.Nombre} de forma permanente" });
                return Unauthorized(new { ok = false, status = 400, msgHeader = "Proyecto no encontrado", msg = "No se encontró el proyecto a eliminar, intenta nuevamente" });
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
