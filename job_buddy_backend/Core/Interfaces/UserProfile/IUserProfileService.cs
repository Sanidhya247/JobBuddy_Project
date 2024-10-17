using job_buddy_backend.DTO.UserProfile;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Interfaces.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto);
        Task<double> CalculateProfileCompletenessAsync(int userId);
        Task<string> UploadProfilePictureAsync(int userId, IFormFile file);
        Task<bool> RemoveProfilePictureAsync(int userId);
        Task<List<UserProfileDto>> GetAllUserProfilesAsync();
    }
}
