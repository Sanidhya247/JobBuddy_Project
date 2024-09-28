using job_buddy_backend.Models;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
