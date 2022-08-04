using Microsoft.AspNetCore.Mvc;
using OauthAPI.ExceptionsHandler;
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
                List<string> errors = new List<string>();
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                foreach (var message in messages)
                {
                    _logger.LogError("[ERROR MESSAGE]: " + message);
                    Console.WriteLine(message.ToString().Trim());
                    errors.Add(message);
                }
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }
    }
}
