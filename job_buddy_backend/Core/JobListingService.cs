using AutoMapper;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;
using JobBuddyBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace job_buddy_backend.Core
{
    public class JobListingService : IJobListingService
    {
        private readonly JobBuddyDbContext _context;
        private readonly IMapper _mapper;

        public JobListingService(JobBuddyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobListingDto> GetJobByIdAsync(int jobId)
        {
            var job = await _context.JobListings.FindAsync(jobId);
            return job == null ? null : _mapper.Map<JobListingDto>(job);
        }

        public async Task<IEnumerable<JobListingDto>> GetAllJobsAsync()
        {
            var jobs = await _context.JobListings.ToListAsync();
            return _mapper.Map<IEnumerable<JobListingDto>>(jobs);
        }

        public async Task<JobListingDto> CreateJobAsync(JobListingDto jobListingDto)
        {
            var job = _mapper.Map<JobListing>(jobListingDto);
            _context.JobListings.Add(job);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobListingDto>(job);
        }

        public async Task<JobListingDto> UpdateJobAsync(int jobId, JobListingDto jobListingDto)
        {
            var existingJob = await _context.JobListings.FindAsync(jobId);
            if (existingJob == null) return null;

            _mapper.Map(jobListingDto, existingJob);
            _context.JobListings.Update(existingJob);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobListingDto>(existingJob);
        }

        public async Task<bool> DeleteJobAsync(int jobId)
        {
            var job = await _context.JobListings.FindAsync(jobId);
            if (job == null) return false;

            _context.JobListings.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<JobListingDto>> SearchJobsAsync(string? title, string? companyName)
        {
            var query = _context.JobListings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(j => j.JobTitle.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                query = query.Where(j => j.Employer.FullName.Contains(companyName));
            }

            var jobs = await query.ToListAsync();
            return _mapper.Map<IEnumerable<JobListingDto>>(jobs);
        }

        public async Task<IEnumerable<JobListingDto>> FilterJobsAsync(string? province, string? city, string? jobType, string? workType, string? experienceLevel, string? industry, decimal? minSalary, decimal? maxSalary)
        {
            var query = _context.JobListings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(province))
            {
                query = query.Where(j => j.Province == province);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                query = query.Where(j => j.City == city);
            }

            if (!string.IsNullOrWhiteSpace(jobType))
            {
                query = query.Where(j => j.JobType == jobType);
            }

            if (!string.IsNullOrWhiteSpace(workType))
            {
                query = query.Where(j => j.WorkType == workType);
            }

            if (!string.IsNullOrWhiteSpace(experienceLevel))
            {
                query = query.Where(j => j.ExperienceLevel == experienceLevel);
            }

            if (!string.IsNullOrWhiteSpace(industry))
            {
                query = query.Where(j => j.Industry == industry);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(j => (j.PayRatePerYear.HasValue && j.PayRatePerYear >= minSalary) ||
                                         (j.PayRatePerHour.HasValue && j.PayRatePerHour * 2080 >= minSalary));
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(j => (j.PayRatePerYear.HasValue && j.PayRatePerYear <= maxSalary) ||
                                         (j.PayRatePerHour.HasValue && j.PayRatePerHour * 2080 <= maxSalary));
            }

            var jobs = await query.ToListAsync();
            return _mapper.Map<IEnumerable<JobListingDto>>(jobs);
        }
    }
}