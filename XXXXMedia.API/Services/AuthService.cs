using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XXXXMedia.Shared.Persistence;
using XXXXMedia.Shared.Persistence.Entities;


namespace XXXXMedia.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly SharedDbContext _db;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(SharedDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed) return null;

            var role = await _db.Roles.FindAsync(user.RoleId);
            var roleName = role?.Name ?? "User";

            var jwtSection = _config.GetSection("JwtSettings");
            var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("Missing JWT Secret");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, ""), 
                new Claim(ClaimTypes.Role, roleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiryMinutes"] ?? "60")),
                signingCredentials: creds
            );

            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> RegisterUserAsync(User user, string password, string roleName = "Admin")
        {
            var existing = await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if (existing != null) return null;

            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
            {
                role = new Role { Name = roleName };
                _db.Roles.Add(role);
                await _db.SaveChangesAsync();
            }

            user.PasswordHash = _hasher.HashPassword(user, password);
            user.RoleId = role.Id;

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
            => await _db.Users.FindAsync(id);
    }
}
