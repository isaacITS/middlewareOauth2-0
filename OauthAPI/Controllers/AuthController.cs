using Microsoft.AspNetCore.Mvc;
using OauthAPI.Tools;

namespace OauthAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SignIn(string email, string pass)
        {
            var user = DbHelper.GetUserByEmail(email);
            if (user == 0)
            {
                return Unauthorized(new { ok = false, msg = "Usuario no registrado" });
            }
            if (!user.Activo)
            {
                return Unauthorized(new { ok = false, msg = "El usuario esta deshabilitado" });
            }
            if (user.Pass != Encrypt.sha256(pass))
            {
                return Unauthorized(new { ok = false, msg = "Contraseña Incorrecta" });
            }
            return Ok(new { ok = true, msg = "Ok", token = "tokenSecret" });
        }
    }
}
