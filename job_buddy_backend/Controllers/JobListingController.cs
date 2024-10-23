using AutoMapper;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingService _jobListingService;
        private readonly IMapper _mapper;

        public JobListingController(IJobListingService jobListingService, IMapper mapper)
        {
            _jobListingService = jobListingService;
            _mapper = mapper;
        }

        [HttpGet("{jobId}")]
        [Authorize(Roles = "Employer, Job Seeker, Admin")]
        public async Task<IActionResult> GetJobById(int jobId)
        {
            var job = await _jobListingService.GetJobByIdAsync(jobId);
            if (job == null)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Job not found."));
            }
            return Ok(ApiResponse<JobListingDto>.SuccessResponse(job));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Employer, Job Seeker")]
        public async Task<IActionResult> GetAllJobs()
        {
            var jobs = await _jobListingService.GetAllJobsAsync();
            return Ok(ApiResponse<IEnumerable<JobListingDto>>.SuccessResponse(jobs));
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] JobListingDto jobListingDto)
        {
            var job = await _jobListingService.CreateJobAsync(jobListingDto);
            return CreatedAtAction(nameof(GetJobById), new { jobId = job.JobID }, ApiResponse<JobListingDto>.SuccessResponse(job, "Job created successfully."));
        }

        [HttpPut("{jobId}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateJob(int jobId, [FromBody] JobListingDto jobListingDto)
        {
            var job = await _jobListingService.UpdateJobAsync(jobId, jobListingDto);
            if (job == null)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Job not found."));
            }
            return Ok(ApiResponse<JobListingDto>.SuccessResponse(job, "Job updated successfully."));
        }

        [HttpDelete("{jobId}")]
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> DeleteJob(int jobId)
        {
            var deleted = await _jobListingService.DeleteJobAsync(jobId);
            if (!deleted)
            {
                return NotFound(ApiResponse<string>.FailureResponse("Job not found."));
            }
            return Ok(ApiResponse<string>.SuccessResponse("Job deleted successfully."));
        }
    }
}