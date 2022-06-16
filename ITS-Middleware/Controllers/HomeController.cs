using System.Data.Entity.Infrastructure;
using ITS_Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }

        [HttpGet]
        public IActionResult Projects()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var data = _context.Proyectos.Where(p => p.Id > 0).ToList();
                return PartialView(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }


        [HttpGet] //Get all Users
        public IActionResult Users()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return RedirectToAction("Login", "Auth");
                }
                var data = _context.Usuarios.Where(u => u.Id >= 2).ToList();
                return PartialView(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }


        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}