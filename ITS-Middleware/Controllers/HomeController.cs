using ITS_Middleware.Models;
using Microsoft.AspNetCore.Mvc;


namespace ITS_Middleware.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public MiddlewareDbContext _context;

        public HomeController(MiddlewareDbContext master, ILogger<HomeController> logger)
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

        public IActionResult Proyects()
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


        [HttpGet] //Get all Users
        public IActionResult Users()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
            {
                return RedirectToAction("Login", "Auth");
            }
            try
            {
                var data = _context.Usuarios.OrderBy(u => u.Id).ToList();
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }
    }
}