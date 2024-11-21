using job_buddy_backend.DTO;
using job_buddy_backend.Models.UserModel;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> RegisterAsync(RegisterUserDto registerDto);
        Task<User> LoginAsync(LoginUserDto loginDto);
        Task<bool> ConfirmEmailAsync(int userId, string token);
        Task<bool> ResetPasswordAsync(int userId, string token, string newPassword);
        string GenerateJwtToken(User user);
        Task<string> GenerateEmailConfirmationTokenAsync(int userId);
        Task<string> GeneratePasswordResetTokenAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
    }
}
