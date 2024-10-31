using job_buddy_backend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IJobListingService
    {
        Task<JobListingDto> GetJobByIdAsync(int jobId);
        Task<IEnumerable<JobListingDto>> GetAllJobsAsync();
        Task<JobListingDto> CreateJobAsync(JobListingDto jobListingDto);
        Task<JobListingDto> UpdateJobAsync(int jobId, JobListingDto jobListingDto);
        Task<bool> DeleteJobAsync(int jobId);
        Task<IEnumerable<JobListingDto>> SearchJobsAsync(string? title, string? companyName, int page, int pageSize);  // Search jobs
        Task<IEnumerable<JobListingDto>> FilterJobsAsync(string? province, string? city, string? jobType, string? workType, string? experienceLevel, string? industry, decimal? minSalary, decimal? maxSalary, int page, int pageSize);  // Filter jobs
    }
}