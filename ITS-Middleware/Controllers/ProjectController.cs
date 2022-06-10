using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITS_Middleware.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Projects()
        {
            return View();
        }

    }
}
