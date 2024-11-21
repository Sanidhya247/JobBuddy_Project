using job_buddy_backend.Core.Interfaces.UserProfile;
using job_buddy_backend.DTO.UserProfile;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IUserProfileService userProfileService, ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProfiles()
        {
            try
            {
                var profiles = await _userProfileService.GetAllUserProfilesAsync();
                return Ok(ApiResponse<List<UserProfileDto>>.SuccessResponse(profiles, "All profiles fetched successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all user profiles.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching all user profiles."));
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var userProfile = await _userProfileService.GetUserProfileAsync(userId);
                return Ok(ApiResponse<UserProfileDto>.SuccessResponse(userProfile, "Profile fetched successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the profile for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching the profile."));
            }
        }

        [HttpPut("{userId}/update-profile")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserProfileDto updateProfileDto)
        {
            try
            {
                var isUpdated = await _userProfileService.UpdateUserProfileAsync(userId, updateProfileDto);
                if (!isUpdated)
                    return BadRequest(ApiResponse<string>.FailureResponse("Failed to update the profile."));

                return Ok(ApiResponse<string>.SuccessResponse("Profile updated successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the profile for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while updating the profile."));
            }
        }

        [HttpPost("{userId}/upload-profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(int userId, [FromForm] ProfilePictureUploadDto pictureDto)
        {
            try
            {
                var profilePictureUrl = await _userProfileService.UploadProfilePictureAsync(userId, pictureDto.ProfilePicture);
                return Ok(ApiResponse<string>.SuccessResponse(profilePictureUrl, "Profile picture uploaded successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while uploading the profile picture for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while uploading the profile picture."));
            }
        }

        [HttpPost("{userId}/upload-cover-photo")]
        public async Task<IActionResult> UploadCoverPhoto(int userId, [FromForm] CoverPhotoUploadDto pictureDto)
        {
            try
            {
                var profilePictureUrl = await _userProfileService.UploadCoverPhotoAsync(userId, pictureDto.CoverPhoto);
                return Ok(ApiResponse<string>.SuccessResponse(profilePictureUrl, "Cover photo uploaded successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while uploading the Cpover photo picture for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while uploading the Cover photo."));
            }
        }

        [HttpDelete("{userId}/remove-profile-picture")]
        public async Task<IActionResult> RemoveProfilePicture(int userId)
        {
            try
            {
                var isRemoved = await _userProfileService.RemoveProfilePictureAsync(userId);
                return Ok(ApiResponse<string>.SuccessResponse("Profile picture removed successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while removing the profile picture for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while removing the profile picture."));
            }
        }

        [HttpGet("{userId}/profile-completeness")]
        public async Task<IActionResult> GetProfileCompleteness(int userId)
        {
            try
            {
                var completeness = await _userProfileService.CalculateProfileCompletenessAsync(userId);
                return Ok(ApiResponse<double>.SuccessResponse(completeness, "Profile completeness calculated successfully."));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while calculating profile completeness for user {userId}.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while calculating profile completeness."));
            }
        }
    }
}
