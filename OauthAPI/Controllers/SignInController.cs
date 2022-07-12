using Microsoft.AspNetCore.Mvc;
using OauthAPI.Models.Entities;
using OauthAPI.Tools;

namespace OauthAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ILogger<SignInController> _logger;

        public SignInController(ILogger<SignInController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SignIn(string email, string pass)
        {
            var trySignIn = DbHelper.SignIn(email, pass);
            if (trySignIn.Ok)
            {
                return Ok(new { ok = true, msg = trySignIn.MsgHeader, msgHeader = trySignIn.Msg });
            }
            return Unauthorized(new { ok = false, msg = trySignIn.MsgHeader, msgHeader = trySignIn.Msg });
        }

        [HttpGet]
        public IActionResult SignInService(string email)
        {
            return Ok(DbHelper.SignInService(email));
        }

        [HttpGet]
        public IActionResult SignInUserProject(string email, string pass)
        {
            return Ok(DbHelper.SignInUserProject(email, pass));
        }

        [HttpPut]
        public IActionResult UpdateToken(string email, string token)
        {
            if (DbHelper.UpdateTokenUser(email, token))
            {
                return Ok(new { ok = true, msg = "Se ha generado el token para cambiar la contraseña", msgHeader = "Token generado" });
            }
            return Unauthorized(new { ok = false, msg = "No se encotró un usaurio registrado, intennta más tarde", msgHeader = "Usuairo no encontrado" });
        }

        [HttpPut]
        public IActionResult UpdatePassword(Usuario usuario)
        {
            if (DbHelper.UpdateUserPassword(usuario))
            {
                return Ok(new { ok = true, msg = $"Hola {usuario.Nombre}, se ha actualizado su contraseña", msgHeader = "Contraseña actualizada" });
            }
            return Unauthorized(new { ok = false, msg = "No se encotró un usaurio registrado, intennta más tarde", msgHeader = "Usuairo no encontrado" });
        }
    }
}
