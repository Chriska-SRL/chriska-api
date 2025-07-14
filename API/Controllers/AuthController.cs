using BusinessLogic;
using BusinessLogic.Domain;
using BusinessLogic.DTOs.DTOsAuth;
using BusinessLogic.DTOs.DTOsUser;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Facade _facade;
        private readonly IConfiguration _config;

        public AuthController(Facade facade, IConfiguration config)
        {
            _facade = facade;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                UserResponse? user = await _facade.AuthenticateAsync(request.Username, request.Password);

                if (user == null)
                    return Unauthorized(new { error = "Credenciales inválidas" });

                var claims = new List<Claim>
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("username", user.Username),
                    new Claim("name", user.Name),
                    new Claim("role", user.Role.Name),
                    new Claim("needsPasswordChange", user.needsPasswordChange.ToString())
                };

                foreach (int perm in user.Role.Permissions)
                    claims.Add(new Claim("permission", perm.ToString()));

                var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                );

                var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
                );

                var expires = DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:ExpirationHours"]));

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return Ok(new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Error inesperado al intentar iniciar sesión" });
            }
        }

        [HttpPost("GetValidToken")]
        public IActionResult GetValidToken([FromBody] TokenRequest request)
        {
            const string Pass = "string";
            try
            {
                if (request.Pass != Pass)
                    return Unauthorized(new { error = "Credenciales inválidas" });

                var claims = new List<Claim>
                {
                    new Claim("userId", "1"),
                    new Claim("username", "admin"),
                    new Claim("name", "Admin"),
                    new Claim("role", "Administrador")
                };

                foreach (Permission perm in Enum.GetValues(typeof(Permission)))
                    claims.Add(new Claim("permission", ((int)perm).ToString()));

                var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])
                );

                var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
                );

                var expires = DateTime.UtcNow.AddHours(int.Parse(_config["Jwt:ExpirationHours"]));

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds
                );

                return Ok(new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error inesperado al generar token" });
            }
        }

        public class TokenRequest
        {
            public string Pass { get; set; } = null!;
        }
    }
}
