using ITS_Middleware.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ITS_Middleware.Tools;
using ITS_Middleware.Models.Entities;

namespace ITS_Middleware.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        public MiddlewareDbContext _context;

        public AuthController(MiddlewareDbContext master, ILogger<AuthController> logger)
        {
            _context = master;
            _logger = logger;
        }


        public IActionResult Login()
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    return View();
                }
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return RedirectToAction("Home", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }



        /*      Methods Requests        */
        //Autenticacion de credenciales
        [HttpPost]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                var user = _context.Usuarios.Where(foundUser => foundUser.Email == email);
                if (user.Any())
                {
                    if (user.Where(s => s.Email == email && s.Pass == pass).Any())
                    {
                        HttpContext.Session.SetString("userEmail", email);
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        ViewBag.msg = "Invalid Password";
                        return View("Login");
                    }
                }
                ViewBag.msg = "User not found";
                return View("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());

                return Json("Error");
            }

        }




        //Clear session
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Remove("userEmail");
                return View("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString().Trim());
                return Json("Error");
            }
        }
    }
}
