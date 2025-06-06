using BusinessLogic;
using BusinessLogic.DTOs.DTOsAuth;
using BusinessLogic.DTOs.DTOsRole;
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
        public IActionResult Login([FromBody] LoginRequest request)
        {
            //UserResponse? user = _facade.Authenticate(request.Username, request.Password);
            UserResponse? user = new UserResponse { 
                Id = 1, Username = "testuser", 
                Name = "Test User", IsEnabled = true, 
                Role = new RoleResponse{ 
                    Id = 1, 
                    Name = "Admin", 
                    Permissions = new List<int> { 1, 2, 3, 4, 5, 6 , 7, 8, 9, 10 } 
                }
            }; // Simulated user for testing
            if (user == null)
                return Unauthorized(new { error = "Credenciales inválidas" });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            foreach (int perm in user.Role.Permissions)
                claims.Add(new Claim("permission", perm.ToString()));

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
            );

            var expirationHours = int.Parse(_config["Jwt:ExpirationHours"]);
            var expires = DateTime.UtcNow.AddHours(expirationHours);

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
    }
}
