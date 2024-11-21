using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using job_buddy_backend.Core;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("user/{userId}/set-status")]
        public async Task<IActionResult> SetUserProfileStatus(int userId, [FromQuery] bool isActive)
        {
            var result = await _adminService.SetUserProfileStatus(userId, isActive);
            return result ? Ok("User status updated.") : NotFound("User not found.");
        }

        [HttpPost("job/{jobId}/set-approval")]
        public async Task<IActionResult> SetJobListingStatus(int jobId, [FromQuery] bool isApproved)
        {
            var result = await _adminService.SetJobListingStatus(jobId, isApproved);
            return result ? Ok("Job listing status updated.") : NotFound("Job listing not found.");
        }

        [HttpDelete("employer/{employerId}")]
        public async Task<IActionResult> DeleteEmployerProfile(int employerId)
        {
            var result = await _adminService.DeleteEmployerProfile(employerId);
            return result ? Ok("Employer profile deleted.") : NotFound("Employer profile not found.");
        }

        [HttpGet("dashboard-statistics")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            var stats = await _adminService.GetDashboardStatistics();
            return Ok(stats);
        }
    }
}
