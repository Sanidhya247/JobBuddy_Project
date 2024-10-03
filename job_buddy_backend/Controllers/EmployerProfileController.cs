using AutoMapper;
using job_buddy_backend.Controllers;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerProfileController : ControllerBase
    {
        private readonly IEmployerProfileService _employerProfileService;
        private readonly IMapper _mapper;

        public EmployerProfileController(IEmployerProfileService employerProfileService, IMapper mapper)
        {
            _employerProfileService = employerProfileService;
            _mapper = mapper;
        }

        [HttpGet("{profileId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetProfile(int profileId)
        {
            var profile = await _employerProfileService.GetProfileByIdAsync(profileId);
            if (profile == null)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Profile not found."));
            }
            return Ok(ApiResponse<EmployerProfileDto>.SuccessResponse(profile));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var profiles = await _employerProfileService.GetAllProfilesAsync();
            return Ok(ApiResponse<IEnumerable<EmployerProfileDto>>.SuccessResponse(profiles));
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateProfile([FromBody] EmployerProfileDto employerProfileDto)
        {
            var profile = await _employerProfileService.CreateProfileAsync(employerProfileDto);
            return CreatedAtAction(nameof(GetProfile), new { profileId = profile.EmployerProfileID }, ApiResponse<EmployerProfileDto>.SuccessResponse(profile, "Profile created successfully."));
        }

        [HttpPut("{profileId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateProfile(int profileId, [FromBody] EmployerProfileDto employerProfileDto)
        {
            var profile = await _employerProfileService.UpdateProfileAsync(profileId, employerProfileDto);
            if (profile == null)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Profile not found."));
            }
            return Ok(ApiResponse<EmployerProfileDto>.SuccessResponse(profile, "Profile updated successfully."));
        }

        [HttpDelete("{profileId}")]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> DeleteProfile(int profileId)
        {
            var deleted = await _employerProfileService.DeleteProfileAsync(profileId);
            if (!deleted)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Profile not found."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Profile deleted successfully."));
        }
    }
}