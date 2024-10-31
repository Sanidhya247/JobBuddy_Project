using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using job_buddy_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using job_buddy_backend.Models.DataContext;

namespace job_buddy_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly JobBuddyDbContext _context;

        public ApplicationsController(JobBuddyDbContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            return await _context.Applications
                                 .Include(a => a.JobListing)
                                 .Include(a => a.JobSeeker)
                                 .Include(a => a.Resume)
                                 .ToListAsync();
        }

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _context.Applications
                                            .Include(a => a.JobListing)
                                            .Include(a => a.JobSeeker)
                                            .Include(a => a.Resume)
                                            .FirstOrDefaultAsync(a => a.ApplicationID == id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // POST: api/Applications
        [HttpPost]
        public async Task<ActionResult<Application>> CreateApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplication), new { id = application.ApplicationID }, application);
        }

        // PUT: api/Applications/5
        [HttpPut("{id}")]
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
