using System.Linq;
using System.Threading.Tasks;
using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;

namespace job_buddy_backend.Core
{
    public class AdminService
    {
        private readonly JobBuddyDbContext _context;

        public AdminService(JobBuddyDbContext context)
        {
            _context = context;
        }

        // Activate/Deactivate User Profile
        public async Task<bool> SetUserProfileStatus(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        // Job Listing  Approve/Disapprove
        public async Task<bool> SetJobListingStatus(int jobId, bool isApproved)
        {
            var jobListing = await _context.JobListings.FindAsync(jobId);
            if (jobListing == null) return false;

            jobListing.IsApproved = isApproved;
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete Employer Profile
        public async Task<bool> DeleteEmployerProfile(int employerId)
        {
            var employer = await _context.EmployerProfiles.FindAsync(employerId);
            if (employer == null) return false;

            _context.EmployerProfiles.Remove(employer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object> GetDashboardStatistics()
        {
            var totalJobs = _context.JobListings.Count();
            var activeJobs = _context.JobListings.Count(j => j.IsApproved);
            var totalUsers = _context.Users.Count();
            var activeUsers = _context.Users.Count(u => u.IsActive);
            var totalApplications = _context.Applications.Count();

            return new
            {
                TotalJobs = totalJobs,
                ActiveJobs = activeJobs,
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalApplications = totalApplications
            };
        }
    }
}
