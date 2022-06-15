using ITS_Middleware.Models.Context;
using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ITS_Middleware.Models.Entities;

namespace ITS_Middleware.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public middlewareITSContext _context;

        public UserController(middlewareITSContext master, ILogger<UserController> logger)
        {
            _context = master;
            _logger = logger;
        }



        public IActionResult Register()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return PartialView();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost]
        public IActionResult Register(Usuario user)
        {
            try
            {
                user.FechaAlta = DateTime.Now;
                user.Activo = true;
                user.Pass = Encrypt.GetSHA256(user.Pass);
                if (ModelState.IsValid)
                {
                    var getEmail = _context.Usuarios.FirstOrDefault(u => u.Email == user.Email);
                    if (getEmail != null)
                    {
                        return Json("Email Existe");
                    }
                    _context.Add(user);
                    _context.SaveChanges();
                    return Json("Usuario registrado");
                }
                return Json(user);
            }
            catch (Exception ex)
            {
                string value = ex.Message.ToString();
                Console.Write(value);
                return Json("Error");
            }
        }


        //Editar usuario
        public IActionResult EditUser(int? id)
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
                    if (id == null || id == 1)
                    {
                        return Json(new { ok = false, msg= "No se ingresó un ID válido o no puede ser editado" });
                    }
                    var usuario = _context.Usuarios.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost]
        public IActionResult UpdateUser(Usuario user)
        {
            try
            {
                user.Pass = SetPassword(user);
                var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChangesAsync();
                return Json(new {ok= true, msg = "Se ha actualizado el usuario con éxito" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }

        /*UPDATE USER STATUS*/
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
                    if (id == null || id == 1)
                    {
                        return Json(new {ok=false, msg = "No se ingresó un ID válido o no puede ser activado/desactivado"});
                    }
                    var usuario = _context.Usuarios.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = false, msg = "El ID no coincide con un usuario registrado" });
                    }
                    return PartialView("ChangeStatus", usuario);
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
        public IActionResult UpdateStatus(Usuario user)
        {
            try
            {
                user.Activo = !user.Activo;
                var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new { ok = true, msg = "Estatus de usuario actualizado" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }



        //Eliminar usuario
        public IActionResult DeleteUser(int? id)
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
                    if (id == null || id == 1)
                    {
                        return Json(new { ok = true, msg = "No se ingresó un ID válido o no puede ser eliminado" });
                    }
                    var usuario = _context.Usuarios.Find(id);
                    if (usuario == null)
                    {
                        return Json(new { ok = true, msg = "El ID No coincide con un usuario registrado" });
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
        public IActionResult DeleteUser([FromBody] int id)
        {
            try
            {
                var user = _context.Usuarios.Find(id);
                if (user != null)
                {
                    _context.Usuarios.Remove(user);
                    _context.SaveChanges();
                    return Json(new { ok = true, msg = "Usuario eliminado con éxito" });
                }
                return Json(new { ok = false, msg = "No se puede eliminar el usuario" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return Json(new { ok = false, msg = "Error" });
                throw;
            }
        }



        private bool UsuarioExiste(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private string SetPassword(Usuario userModel)
        {
            if (string.IsNullOrEmpty(userModel.Pass) || userModel.Pass.Length < 8)
            {
                var getUsrData = _context.Usuarios.FirstOrDefault(u => u.Id == userModel.Id);
                if (getUsrData != null) return getUsrData.Pass;
            }
            return Tools.Encrypt.GetSHA256(userModel.Pass);
        }

        private bool SetStatus(Usuario userModel)
        {
            if (userModel.Activo)
            {
                return false;
            }
            return true;
        }
    }
}
