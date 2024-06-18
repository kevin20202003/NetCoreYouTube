using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCoreYouTube.Models;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NetCoreYouTube.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult IniciarSesion([FromBody] LoginRequest loginRequest)
        {
            // Validar si el JSON se deserializa correctamente
            if (loginRequest == null)
            {
                return BadRequest("Petición no válida");
            }

            string user = loginRequest.usuario;
            string password = loginRequest.password;

            Usuario usuario = Usuario.Db().FirstOrDefault(x => x.Nombre == user && x.Password == password);

            if (usuario == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                    results = ""
                });
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", usuario.Id),
                new Claim("Nombre", usuario.Nombre),
                new Claim("Rol", usuario.Rol) // Añadir el rol del usuario como claim si es necesario
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signIn
            );

            return Ok(new
            {
                success = true,
                message = "Éxito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }

    // Clase para modelar la estructura esperada del JSON de login
    public class LoginRequest
    {
        public string usuario { get; set; }
        public string password { get; set; }
    }
}
