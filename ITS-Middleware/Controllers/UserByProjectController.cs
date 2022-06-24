using ITS_Middleware.ExceptionsHandler;
using ITS_Middleware.Models.Context;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITS_Middleware.Controllers
{
    public class UserByProjectController : Controller
    {
        private readonly ILogger<UserByProjectController> _logger;
        public middlewareITSContext _context;

        public UserByProjectController(middlewareITSContext master, ILogger<UserByProjectController> logger)
        {
            _context = master;
            _logger = logger;
        }

        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var projects = _context.Proyectos.Where(p => p.Id > 0).ToList();
                ViewData["ProjectsList"] = projects;
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
        public IActionResult Register(UsuariosProyecto user)
        {
            try
            {
                user.FechaCreacion = DateTime.Now;
                user.FechaAcceso = DateTime.Now;
                user.Pass = Encrypt.sha256(user.Pass);
                if (ModelState.IsValid)
                {
                    var getEmail = _context.UsuariosProyectos.FirstOrDefault(u => u.Email == user.Email);
                    if (getEmail != null)
                    {
                        return Json(new { ok = false, status = 410, msg = $"El correo {user.Email} ya esta registrado" });
                    }
                    _context.Add(user);
                    _context.SaveChanges();
                    return Json(new { ok = true, status = 200, msg = $"Se ha registrado el usuario {user.NombreCompleto}" });
                }
                return Json(user);
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


        //Editar usuario
        public IActionResult Update(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = _context.UsuariosProyectos.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    var projects = _context.Proyectos.Where(p => p.Id > 0).ToList();
                    ViewData["ProjectsList"] = projects;
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
        public IActionResult Update(UsuariosProyecto user)
        {
            try
            {
                user.Pass = SetPassword(user);
                var local = _context.Set<UsuariosProyecto>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { ok = true, msg = "Se ha actualizado el usuario con éxito" });
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

        //Eliminar usuario
        public IActionResult Delete(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var usuario = _context.UsuariosProyectos.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = true, msg = "El ID No coincide con un usuario registrado" });
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
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                var user = _context.UsuariosProyectos.Find(id);
                if (user != null)
                {
                    _context.UsuariosProyectos.Remove(user);
                    _context.SaveChanges();
                    return Json(new { ok = true, msg = "Usuario eliminado con éxito" });
                }
                return Json(new { ok = false, msg = "No se logró encontrar el usuario" });
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


        private string SetPassword(UsuariosProyecto userModel)
        {
            if (userModel.Pass == "0000000000" || userModel.Pass.Length < 8 || string.IsNullOrEmpty(userModel.Pass))
            {
                var getUsrData = _context.UsuariosProyectos.FirstOrDefault(u => u.Id == userModel.Id);
                if (getUsrData != null) return getUsrData.Pass;
            }
            return Encrypt.sha256(userModel.Pass);
        }
    }
}
