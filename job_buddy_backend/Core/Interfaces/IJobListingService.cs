using job_buddy_backend.DTO;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IJobListingService
    {

        Task<IEnumerable<JobListingDto>> GetAllJobsAsync();
        Task<JobListingDto> GetJobByIdAsync(int jobId);
        Task<JobListingDto> CreateJobAsync(JobListingDto jobListingDto);
        Task<JobListingDto> UpdateJobAsync(int jobId, JobListingDto jobListingDto);
        Task<bool> DeleteJobAsync(int jobId);
    }
}
