using System.Data.Entity.Infrastructure;
using ITS_Middleware.Models;
using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ITS_Middleware.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public MiddlewareDbContext _context;

        public UserController(MiddlewareDbContext master, ILogger<UserController> logger)
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
        public async Task<IActionResult> Register(Usuario user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getEmail = _context.Usuarios.FirstOrDefault(u => u.Email == user.Email);
                    if (getEmail != null)
                    {
                        return Json($"El Correo {user.Email} ya esta registrado");
                    }
                    var passHashed = Encrypt.GetSHA256(user.Pass);
                    user.Pass = passHashed;
                    user.Activo = true;
                    _context.Add(user);

                    await _context.SaveChangesAsync();
                    return Json("Usuario almacenado correctamente");
                }
                return Json("El modelo no es valido", user);
            }
            catch (Exception ex)
            {
                string value = ex.Message.ToString();
                Console.Write(value);
                return Json("Error");
            }
        }


        //Editar usuario
        public async Task<IActionResult> EditUser(int? id)
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
                        ViewBag.msg = "No se ingresó un ID válido o no puede ser editado";
                        return View();
                    }
                    var usuario = await _context.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        ViewBag.msg = "El ID no coincide con un usuario registrado";
                        return View();
                    }
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(Usuario user)
        {
            try
            {
                user.Pass = SetPassword(user);
                var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Users", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
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
                        ViewBag.msg = "No se ingresó un ID válido o no puede ser activado/desactivado";
                        return View("ChangeStatus");
                    }
                    var usuario = _context.Usuarios.Find(id);
                    if (usuario == null)
                    {
                        ViewBag.msg = "El ID no coincide con un usuario registrado";
                        return View("ChangeStatus");
                    }
                    return View("ChangeStatus", usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Usuario user)
        {
            try
            {
                user.Activo = !user.Activo;
                var local = _context.Set<Usuario>().Local.FirstOrDefault(entry => entry.Id.Equals(user.Id));
                if (local != null) _context.Entry(local).State = EntityState.Detached;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Users", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }



        //Eliminar usuario
        public async Task<IActionResult> DeleteUser(int? id)
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
                        ViewBag.msg = "No se ingresó un ID válido o no puede ser eliminado";
                        return View();
                    }
                    var usuario = await _context.Usuarios.FindAsync(id);
                    if (usuario == null)
                    {
                        ViewBag.msg = "El ID No coincide con un usuario registrado";
                        return View();
                    }
                    return View(usuario);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Usuarios.FindAsync(id);
                if (user != null)
                {
                    _context.Usuarios.Remove(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Users", "Home");
                }
                return RedirectToAction("Users", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }



        private bool UsuarioExiste(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private string SetPassword(Usuario userModel)
        {
            if (string.IsNullOrEmpty(userModel.Pass))
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
