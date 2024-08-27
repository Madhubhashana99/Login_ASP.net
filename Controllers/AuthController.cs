using Login_new.Data;
using Login_new.DTO;
using Login_new.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using BCrypt.Net; // For password hashing

namespace Login_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelpers _jwtHelper;

        public AuthController(ApplicationDbContext context, JwtHelpers jwtHelper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtHelper = jwtHelper ?? throw new ArgumentNullException(nameof(jwtHelper));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("User does not exist.");
            }

            // Direct comparison of plain text passwords (No hashing)
            if (user.Password == null || user.Password != request.Password)
            {
                return Unauthorized("Invalid password.");
            }

            var token = _jwtHelper.GenerateJwtToken(user);
            return Ok(new { token });
        }
    }
}
