using Hospital_ManagementSystem_Api.DBContext;
using Hospital_ManagementSystem_Api.DTOs.AuthDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital_ManagementSystem_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController( ApplicationDbContext context
                               , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email
                                                            && u.Password == loginDTO.Password
                                                            );

            if (user == null)
            {
                return Unauthorized(new { Message = "User Not Found" });
            }

            // Get user's role
            var userRole = await _context.userRoles
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            if (userRole == null || userRole.Role == null)
            {
                return BadRequest(new { Message = "Role not assigned to this user." });
            }

            // JWT Claims
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserId!.ToString() ?? string.Empty),
        new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Role, userRole.Role.RoleName!),
    };

            // Secret Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Create Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
    Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = jwtToken,
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = userRole.Role.RoleName
            });
        }

    }
}
