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
                return Ok(authMethods);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }
    }
}
