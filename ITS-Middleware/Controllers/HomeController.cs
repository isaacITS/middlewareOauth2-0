using System.Data.Entity.Infrastructure;
using ITS_Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ITS_Middleware.Models.Context;
using ITS_Middleware.ExceptionsHandler;

namespace ITS_Middleware.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public middlewareITSContext _context;

        public HomeController(middlewareITSContext master, ILogger<HomeController> logger)
        {
            _context = master;
            _logger = logger;
        }

        public IActionResult Home()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                ViewBag.userName = HttpContext.Session.GetString("userName");
                return View();
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

        [HttpGet] //Get all projects
        public IActionResult Projects()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var projects = _context.Proyectos.Where(p => p.Id > 0).ToList();
                var users = _context.Usuarios.Where(u => u.Id > 0).ToList();
                ViewData["UserList"] = users;
                return PartialView(projects);
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


        [HttpGet] //Get all Users
        public IActionResult Users()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var data = _context.Usuarios.Where(u => u.Id >= 2).ToList();
                return PartialView(data);
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


        //Get and return view usersByProject with all data registered in DB
        [HttpGet]
        public IActionResult UsersByProject()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userName")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var usersByProject = _context.UsuariosProyectos.Where(u => u.Id > 0).ToList();
                var projects = _context.Proyectos.Where(p => p.Id > 0).ToList();
                ViewData["ProjectsList"] = projects;
                return PartialView(usersByProject);
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

        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}