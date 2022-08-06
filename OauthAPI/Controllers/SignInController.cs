using Microsoft.AspNetCore.Mvc;
using OauthAPI.ExceptionsHandler;
using OauthAPI.Helpers;
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
                    trySignIn.Token = JwtToken.GenerateToken();
                    return Ok(trySignIn);
                }
                return Unauthorized(trySignIn);

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


        [HttpPost]
        public async Task<IActionResult> SignInUserProject(SigninData signinData)
        {
            try
            {
                if (string.IsNullOrEmpty(signinData.Pass)) return Ok(await DbHelper.SignInService(signinData));
                
                return Ok(DbHelper.SignInUserProject(signinData));
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

        [HttpPut]
        public IActionResult UpdateToken(string email, string token)
        {
            try
            {
                if (DbHelper.UpdateTokenUser(email, token))
                {
                    return Ok(new { ok = true, status = 200, msg = "Se ha enviado un correo con un token para actualizar la contraseña", msgHeader = $"Correo enviado a {email}" });
                }
                throw new Exception();
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

        [HttpPut]
        public IActionResult UpdatePassword(Usuario usuario)
        {
            try
            {
                if (DbHelper.UpdateUserPassword(usuario))
                {
                    return Ok(new { ok = true, status = 200, msg = $"Hola {usuario.Nombre}, se ha actualizado su contraseña", msgHeader = "Contraseña actualizada" });
                }
                throw new Exception();
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
