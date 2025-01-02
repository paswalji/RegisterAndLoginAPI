using LoginWebAPI.Data;
using LoginWebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == login.Username && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User newUser)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == newUser.Username);
            if (userExists)
                return BadRequest("Username already exists");

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Registration successful");
        }
    }
}
