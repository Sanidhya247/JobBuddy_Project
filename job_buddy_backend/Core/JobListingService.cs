using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models.DataContext;
using JobBuddyBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Core
{
    public class JobListingService : IJobListingService
    {
        private readonly JobBuddyDbContext _context;

        public JobListingService(JobBuddyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobListingDto>> GetAllJobsAsync()
        {
            return await _context.JobListings
                .Select(job => new JobListingDto
                {
                    JobID = job.JobID,
                    JobTitle = job.JobTitle,
                    ShortJobDescription = job.ShortJobDescription,
                    JobDescription = job.JobDescription,
                    City = job.City,
                    Province = job.Province,
                    ZipCode = job.ZipCode,
                    SalaryRange = job.SalaryRange,
                    PayRatePerYear = job.PayRatePerYear,
                    PayRatePerHour = job.PayRatePerHour,
                    JobType = job.JobType,
                    WorkType = job.WorkType
                }).ToListAsync();
        }

        public async Task<JobListingDto> GetJobByIdAsync(int jobId)
        {
            var job = await _context.JobListings.FindAsync(jobId);
            if (job == null) return null;

            return new JobListingDto
            {
                JobID = job.JobID,
                JobTitle = job.JobTitle,
                ShortJobDescription = job.ShortJobDescription,
                JobDescription = job.JobDescription,
                City = job.City,
                Province = job.Province,
                ZipCode = job.ZipCode,
                SalaryRange = job.SalaryRange,
                PayRatePerYear = job.PayRatePerYear,
                PayRatePerHour = job.PayRatePerHour,
                JobType = job.JobType,
                WorkType = job.WorkType
            };
        }

        public async Task<JobListingDto> CreateJobAsync(JobListingDto jobListingDto)
        {
            var job = new JobListing
            {
                JobTitle = jobListingDto.JobTitle,
                ShortJobDescription = jobListingDto.ShortJobDescription,
                JobDescription = jobListingDto.JobDescription,
                City = jobListingDto.City,
                Province = jobListingDto.Province,
                ZipCode = jobListingDto.ZipCode,
                SalaryRange = jobListingDto.SalaryRange,
                PayRatePerYear = jobListingDto.PayRatePerYear,
                PayRatePerHour = jobListingDto.PayRatePerHour,
                JobType = jobListingDto.JobType,
                WorkType = jobListingDto.WorkType
            };

            _context.JobListings.Add(job);
            await _context.SaveChangesAsync();

            return jobListingDto;
        }

        public async Task<JobListingDto> UpdateJobAsync(int jobId, JobListingDto jobListingDto)
        {
            var job = await _context.JobListings.FindAsync(jobId);
            if (job == null) return null;

            job.JobTitle = jobListingDto.JobTitle;
            job.ShortJobDescription = jobListingDto.ShortJobDescription;
            job.JobDescription = jobListingDto.JobDescription;
            job.City = jobListingDto.City;
            job.Province = jobListingDto.Province;
            job.ZipCode = jobListingDto.ZipCode;
            job.SalaryRange = jobListingDto.SalaryRange;
            job.PayRatePerYear = jobListingDto.PayRatePerYear;
            job.PayRatePerHour = jobListingDto.PayRatePerHour;
            job.JobType = jobListingDto.JobType;
            job.WorkType = jobListingDto.WorkType;

            await _context.SaveChangesAsync();
            return jobListingDto;
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var job = await _context.JobListings.FindAsync(jobId);
            if (job == null) return false;

            _context.JobListings.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
