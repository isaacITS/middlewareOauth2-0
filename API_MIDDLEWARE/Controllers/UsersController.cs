using API_MIDDLEWARE.Models;
using API_MIDDLEWARE.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using API_MIDDLEWARE.Tools;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace API_MIDDLEWARE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        public middlewareITSContext _context;
        private readonly IConfiguration _configuration;


        public UsersController(middlewareITSContext master, ILogger<UsersController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = master;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuarios>>> Get()
        {
            
            return Ok(await _context.Usuarios.ToListAsync());
        }


        [HttpPost("login")]
        public IActionResult Login(string email, string pass)
        {
            var user = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Usuario no registrado");
            }
            if (!user.Activo)
            {
                return BadRequest("Usuario no Encontrado");
            }
            if (user.Pass != Encrypt.GetSHA256(pass))
            {
                return BadRequest("Contraseña incorrecta");
            }
       
            string token = CreateToken(user);

            return Ok(token);
        }


        
        private string CreateToken(Usuarios user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nombre),
                //new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }




    }
}
