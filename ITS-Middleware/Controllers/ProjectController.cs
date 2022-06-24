using ITS_Middleware.Models.Entities;
using ITS_Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ITS_Middleware.Models.Context;
using ITS_Middleware.ExceptionsHandler;

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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var metodos = _context.MetodosAuths.Where(m => m.Id > 0).ToList();
                ViewData["MetodosAuth"] = metodos;
                return PartialView();
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
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }


        [HttpPost]
        public IActionResult Register(Proyecto project)
        {
            try
            {
                project.Activo = true;
                project.IdUsuarioRegsitra = int.Parse(HttpContext.Session.GetString("idUser"));
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
                return Json(new { ok = false, msg = "Información incompleta, intenta volver a iniciar sesión"});
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
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }

        //Editar proyecto
        public IActionResult EditProject(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
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
                    var metodos = _context.MetodosAuths.Where(m => m.Id > 0).ToList();
                    ViewData["MetodosAuth"] = metodos;
                    return PartialView(project);
                }
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
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }

        [HttpPost]
        public IActionResult UpdateProject(Proyecto project)
        {
            try
            {
                var local = _context.Set<Proyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(project.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(project).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { ok = true, msg = "Información de Proyecto Actualizada" });
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
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }


        //Eliminar proyecto
        public IActionResult DeleteProject(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
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
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
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
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }

        //Estatus activo/desactivo proyectos
        public IActionResult UpdateStatus(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ViewBag.email = HttpContext.Session.GetString("userName");
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
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
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
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                TempData["ErrorsMessages"] = errors;
                return Json(new { ok = false, status = 500, msg = "Error" });
            }
        }
    }
}
