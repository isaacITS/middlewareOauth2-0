using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ITS_Middleware.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Models.User user = new Models.User();
        private readonly IConfiguration config;


        public AuthController(IConfiguration configuraiton)
        {
            config = configuraiton;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Models.User>> Register(UserInput req)
        {
            user.Username = req.Username;
            user.Password = Tools.Encrypt.GetSHA256(req.Password);

            return Ok(user);
        }

        //Autenticacion de usaurio
        [HttpPost("auth")]
        public async Task<ActionResult<string>> Auth(UserInput req)
        {
            if (user.Username != req.Username)
            {
                return BadRequest("User Not found");
            }
            if (user.Password != Tools.Encrypt.GetSHA256(req.Password))
            {
                return BadRequest("Incorrect password");
            }

            string token = GenerateToken(user);
            return Ok(token);
        }



        //Generar token
        private string GenerateToken(Models.User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username, user.Password),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                config.GetSection("appSettings:token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        
    }
}
