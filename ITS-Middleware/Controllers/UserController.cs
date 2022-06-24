﻿using ITS_Middleware.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ITS_Middleware.Models.Entities;
using ITS_Middleware.Models.Context;
using ITS_Middleware.ExceptionsHandler;

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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
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
        public IActionResult Register(Usuario user)
        {
            try
            {
                user.FechaAlta = DateTime.Now;
                user.Activo = true;
                user.Pass = Encrypt.sha256(user.Pass);
                if (ModelState.IsValid)
                {
                    var getEmail = _context.Usuarios.FirstOrDefault(u => u.Email == user.Email);
                    if (getEmail != null)
                    {
                        return Json(new { ok = false, status = 410, msg = $"No se registró el usuario {user.Nombre}" });
                    }
                    _context.Add(user);
                    _context.SaveChanges();
                    return Json(new { ok = true, status = 200, msg = $"Se ha registrado el usuario {user.Nombre}" });
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
        public IActionResult EditUser(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
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

        /*UPDATE USER STATUS*/
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
        public IActionResult DeleteUser(int? id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
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



        private bool getUserById(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private string SetPassword(Usuario userModel)
        {
            if (userModel.Pass == "0000000000" || userModel.Pass.Length < 8 || string.IsNullOrEmpty(userModel.Pass))
            {
                var getUsrData = _context.Usuarios.FirstOrDefault(u => u.Id == userModel.Id);
                if (getUsrData != null) return getUsrData.Pass;
            }
            return Encrypt.sha256(userModel.Pass);
        }
    }
}
