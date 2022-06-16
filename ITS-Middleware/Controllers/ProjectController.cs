using ITS_Middleware.Models.Entities;
using ITS_Middleware.Models;
using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ITS_Middleware.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        public middlewareITSContext _context;

        public ProjectController(middlewareITSContext master, ILogger<ProjectController> logger)
        {
            _context = master;
            _logger = logger;
        }

        //Registrar proyecto
        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }


        [HttpPost]
        public IActionResult Register(Proyecto project)
        {
            try
            {
                project.Activo = true;
                project.IdUsuarioRegsitra = int.Parse(HttpContext.Session.GetString("idUser"));
                project.Pass = Encrypt.GetSHA256(project.Pass);
                project.FechaAlta = DateTime.Now;
                if (ModelState.IsValid)
                {
                    var getName = _context.Proyectos.Where(p => p.Nombre == project.Nombre).FirstOrDefault();
                    if (getName != null)
                    {
                        return Json(new { ok = false, msg = $"El proyecto {project.Nombre} ya esta registrado" });
                    }
                    _context.Add(project);
                    _context.SaveChanges();
                    return Json(new { ok = true, msg = $"El proyecto {project.Nombre} se ha registrado" });
                }
                return Json(new { ok = false, msg = "El modelo no es válido"});
            }
            catch (Exception ex)
            {
                string value = ex.Message.ToString();
                Console.Write(value);
                return Json(new { ok = false, msg = "Error" });
            }
        }

        //Editar proyecto
        public IActionResult EditProject(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var project = _context.Proyectos.Find(id);
                    if (project == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un proyecto registrado" });
                    }
                    return PartialView(project);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }

        [HttpPost]
        public IActionResult UpdateProject(Proyecto project)
        {
            try
            {
                project.Pass = SetPassProject(project);
                var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(project.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { ok = true, msg = "Información de Proyecto Actualizada" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }


        //Eliminar proyecto
        public IActionResult DeleteProject(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var usuario = _context.Proyectos.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID No coincide con un proyecto registrad" });
                    }
                    return PartialView(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }



        [HttpPost]
        public IActionResult DeleteProject([FromBody] int id)
        {
            try
            {
                var project = _context.Proyectos.Find(id);
                if (project != null)
                {
                    _context.Proyectos.Remove(project);
                    _context.SaveChanges();
                    return Json(new { ok = true, msg = "Proyecto eliminado con éxito" });
                }
                return Json(new { ok = false, msg = "No se encontró un proyecto" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }

        //Estatus activo/desactivo proyectos
        public IActionResult UpdateStatus(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ViewBag.email = HttpContext.Session.GetString("userEmail");
                    if (id == null)
                    {
                        return Json(new { ok = false, msg = "No se ingresó un ID válido" });
                    }
                    var project = _context.Proyectos.Find(id);
                    if (project == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView("ChangeStatus", project);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }

        [HttpPost]
        public IActionResult UpdateStatus(Proyecto project)
        {
            try
            {
                project.Activo = !project.Activo;
                var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(project.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { ok = true, msg = "Estatus de proyecto Actualizado" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }


        private string SetPassProject(Proyecto projectModel)
        {
            if (string.IsNullOrEmpty(projectModel.Pass))
            {
                var getProjectData = _context.Proyectos.FirstOrDefault(p => p.Id == projectModel.Id);
                if (getProjectData != null) return getProjectData.Pass;
            }
            return Tools.Encrypt.GetSHA256(projectModel.Pass);
        }

    }
}
