using job_buddy_backend.DTO.UserProfile;

namespace job_buddy_backend.Core.Interfaces.UserProfile
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto);
        Task<double> CalculateProfileCompletenessAsync(int userId);
    }
}
