using job_buddy_backend.Models.UserModel;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user);
    }
}
