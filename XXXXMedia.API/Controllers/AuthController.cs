using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using XXXXMedia.API.Services;
using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        public AuthController(IAuthService auth) { _auth = auth; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _auth.AuthenticateAsync(req.Username, req.Password);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var user = new User
            {
                Username = req.Username
            };
            var created = await _auth.RegisterUserAsync(user, req.Password, req.Role ?? "Admin");
            if (created == null) return BadRequest(new { message = "User exists" });
            return CreatedAtAction(nameof(Register), new { id = created.Id }, new { created.Id });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(sub, out var id)) return Unauthorized();
            var user = await _auth.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(new { user.Id, user.Username, user.RoleId });
        }
    }

    public record LoginRequest(string Username, string Password);
    public record RegisterRequest(string Username, string Password, string? Role);
}
