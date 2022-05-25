using ITS_Middleware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ITS_Middleware.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private AccountService accountService;

        public AuthController(AccountService acntService)
        {
            accountService = acntService;
        }

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

        [HttpPost("login")]
        public IActionResult Login(string email, string pass)
        {
            var user = accountService.Login(email, pass);
            if (user == null)
            {
                ViewBag.msg = "Invalid Email or Password";
                return View("login");
            }
            HttpContext.Session.SetString("userEmail", email);
            return RedirectToAction("home");
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


        [Route("logout")]
        public IActionResult Logout()
        {
            Console.Write("Ok method");
            HttpContext.Session.Remove("userEmail");
            return View("login");
        }
    }
}
