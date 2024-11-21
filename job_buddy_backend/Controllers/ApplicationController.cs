using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using job_buddy_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using job_buddy_backend.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using job_buddy_backend.DTO;

namespace job_buddy_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ApplicationsController : ControllerBase
    {
        private readonly JobBuddyDbContext _context;
        private readonly ILogger<ApplicationsController> _logger;
        public ApplicationsController(JobBuddyDbContext context, ILogger<ApplicationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Applications
        [HttpGet]
        [Authorize(Roles = "Employer, Job Seeker, Admin")]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications
                                 .Include(a => a.JobListing)
                                 .Include(a => a.JobSeeker)
                                 .Include(a => a.Resume)
                                 .ToListAsync();
        }

        // GET: api/Applications/5
        [HttpGet("{userId}/{jobId}")]
        [Authorize(Roles = "Employer, Job Seeker, Admin")]
        public async Task<IActionResult> GetApplication(int userId, int jobId)
        {
            try
            {
                var application = await _context.Applications
                    .Include(a => a.JobListing)
                    .Include(a => a.JobSeeker)
                    .Include(a => a.Resume)
                    .FirstOrDefaultAsync(a => a.JobID == jobId && a.UserID == userId);

                if (application == null)
                {
                    return Ok(ApiResponse<Application>.FailureResponse("Application not found."));
                }

                return Ok(ApiResponse<Application>.SuccessResponse(application, "Application fetched successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the application.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while fetching the application."));
            }
        }


        // POST: api/Applications
        [HttpPost]
        [Authorize(Roles = "Job Seeker")]
        public async Task<ActionResult<Application>> CreateApplication(Application application)
        {
            var user = await _context.Users.FindAsync(application.UserID);
            if (user == null)
            {
                return BadRequest("Invalid User ID.");
            }

            var jobListing = await _context.JobListings.FindAsync(application.JobID);
            if (jobListing == null)
            {
                return BadRequest("Invalid Job ID.");
            }

         
            var resume = await _context.Resumes.FindAsync(application.ResumeID);
            if (resume == null)
            {
                return BadRequest("Invalid Resume ID.");
            }

            application.JobSeeker = user;
            application.JobListing = jobListing;
            application.Resume = resume;

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

           

            return Ok(ApiResponse<Application>.SuccessResponse(application, "Application submitted successfully."));

            
        }


        // PUT: api/Applications/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Job Seeker")]
        public async Task<IActionResult> UpdateApplication(int id, Application application)
        {
            if (id != application.ApplicationID)
            {
                return BadRequest("Application ID mismatch.");
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Applications/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer, Job Seeker, Admin")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.ApplicationID == id);
        }
    }
}
