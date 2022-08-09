using Microsoft.AspNetCore.Authorization;
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
        private readonly IHostEnvironment _env;
        private readonly IConfiguration config;

        public SignInController(ILogger<SignInController> logger, IHostEnvironment env, IConfiguration config)
        {
            _logger = logger;            _env = env;
            this.config = config;
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
        public IActionResult UpdateToken(string email, string token, string siteUrl)
        {
            try
            {
                if (!ValidateTokenEncrypted.TokenIsValid(token)) return Ok(new { ok = false, status = 400, msg = "La información que se esta ingresando no es correcta, verifica los datos", msgHeader = "No se ha enviado el correo electrónico" });
                var user = DbHelper.GetUserByEmail(email);
                if (user == null || !user.Activo) return Ok(new { ok = false, status = 400, msg = "Al parecer no existe un usuario registrado o se encuentra deshabilitado", msgHeader = "No se ha enviado el correo electrónico" });
                user.TokenRecovery = token;
                if (DbHelper.UpdateUser(user))
                {
                    var tokenJwt = JwtToken.GenerateTokenUpdatePass();
                    var bodyEmail = System.IO.File.ReadAllText(Path.Combine(_env.ContentRootPath, @"EmailViewTemplates\UserAdmin\resetPassMessage.html"))
                    .Replace("{contact-name}", user.Nombre)
                    .Replace("{link-update-pass}", $"{siteUrl}?token={token}&jwt={tokenJwt}");
                    SendEmail.SendEmailReq(email, bodyEmail, "Recuperación de contraseña, portal de administración Oauth2.0",
                        config.GetSection("ApplicationSettings:EmailConfig:email").Value.ToString(),
                        config.GetSection("ApplicationSettings:EmailConfig:password").Value.ToString());
                    return Ok(new { ok = true, status = 200, msg = $"Hola {user.Nombre}, te hemos enviado un correo electrónico", msgHeader = "Correo electrónico enviado con exito" });
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
        [Authorize]
        public IActionResult UpdatePassword(Usuario usuario)
        {
            try
            {
                usuario.TokenRecovery = null;
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
