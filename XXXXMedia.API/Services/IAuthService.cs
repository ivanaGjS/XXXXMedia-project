using XXXXMedia.Shared.Persistence.Entities;

namespace XXXXMedia.API.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(string username, string password);
        Task<User?> RegisterUserAsync(User user, string password, string roleName = "Admin");
        Task<User?> GetUserByIdAsync(int id);
    }
}
