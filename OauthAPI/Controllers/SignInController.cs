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
            try
            {
                var trySignIn = DbHelper.SignIn(email, pass);
                if (trySignIn.Ok)
                {
                    return Ok(trySignIn);
                }
                return Unauthorized(trySignIn);

            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult SignInService(string email, string phoneNumber)
        {
            try
            {
                return Ok(DbHelper.SignInService(email, phoneNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpGet]
        public IActionResult SignInUserProject(string email, string pass)
        {
            try
            {
                return Ok(DbHelper.SignInUserProject(email, pass));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult UpdateToken(string email, string token)
        {
            try
            {
                if (DbHelper.UpdateTokenUser(email, token))
                {
                    return Ok(new { ok = true, status = 200, msg = "Se ha enviado un correo con un token para actualizar la contraseña", msgHeader = $"Correo enviado a {email}" });
                }
                return Unauthorized(new { ok = false, status = 400, msg = $"No se encotró un usuario registrado con el correo {email}", msgHeader = "Usuairo no encontrado" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }

        [HttpPut]
        public IActionResult UpdatePassword(Usuario usuario)
        {
            try
            {
                if (DbHelper.UpdateUserPassword(usuario))
                {
                    return Ok(new { ok = true, status = 200, msg = $"Hola {usuario.Nombre}, se ha actualizado su contraseña", msgHeader = "Contraseña actualizada" });
                }
                return Unauthorized(new { ok = false, status = 400, msg = "No se encotró un usuario registrado", msgHeader = "Usuairo no encontrado" });
            }
            catch (Exception ex)
            {
               return BadRequest(new { ok = false, status = 500, msg = ex.Message.ToString() });
            }
        }
    }
}
