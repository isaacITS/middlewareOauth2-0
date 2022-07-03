using Microsoft.AspNetCore.Mvc;
using OauthAPI.Tools;

namespace OauthAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OauthMethodController : ControllerBase
    {
        private readonly ILogger<OauthMethodController> _logger;

        public OauthMethodController(ILogger<OauthMethodController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var authMethods = DbHelper.GetAllAuthMethods();
                if (authMethods != null)
                {
                    return Ok(authMethods);
                }
                return Unauthorized(new { ok = false, msg = "Error al obtener los metodos Oauth" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { ok = false, msg = ex.Message.ToString() });
            }
        }
    }
}
