using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InventorySystemAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string USERNAME = "admin";
        private const string PASSWORD = "admin123";
        private const string SECRET = "THIS_IS_A_DEV_ONLY_SECRET_KEY_32_CHARS_MIN"; // demo only

        [HttpPost("login")]

        public IActionResult Login(LoginRequestDto dto)
        {
            if (dto.Username != USERNAME || dto.Password != PASSWORD)
                return Unauthorized();

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, dto.Username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return Ok(new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
