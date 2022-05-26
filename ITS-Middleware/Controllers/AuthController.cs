using ITS_Middleware.Models;
using ITS_Middleware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ITS_Middleware.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        //private AccountService accountService;

        public UsersContext _context;

        public AuthController(/*AccountService acntService*/ UsersContext master)
        {
            this._context = master;
            //accountService = acntService;
        }


        /*      Routes      */
        [Route("")]
        [Route("~/")]
        [Route("login")]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("userEmail") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return View("home");
            }
            return View();
        }


        [Route("home")]
        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("userEmail") != null)
            {
                ViewBag.email = HttpContext.Session.GetString("userEmail");
                return View("home");
            }
            return View("login");
        }




        /*      Methods Requests        */
        //Autenticacion de credenciales
        [HttpPost("login")]
        public IActionResult Login(string email, string pass)
        {
            var user = _context.Users.Where(foundUser => foundUser.Email == email);
            //var user = accountService.Login(email, pass);
            if (user.Any())
            {
                if (user.Where(s => s.Email == email && s.Pass == pass).Any())
                {
                    HttpContext.Session.SetString("userEmail", email);
                    return RedirectToAction("home");
                }
                else
                {
                    ViewBag.msg = "Invalid Password";
                    return View("login");
                }
            }
            ViewBag.msg = "User not found";
            return View("login");
        }


        //Clear session
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userEmail");
            return View("login");
        }
    }
}
