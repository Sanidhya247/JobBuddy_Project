using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly JobBuddyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ResumesController(JobBuddyDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile resume, int userId)
        {
            if (resume == null || resume.Length == 0)
                return BadRequest("No file uploaded");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound("User not found");

            // Save the file locally
            var uploads = Path.Combine(_environment.WebRootPath, "resumes");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, resume.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(fileStream);
            }

            // Save resume data in the database
            var newResume = new Resume
            {
                UserID = userId,
                ResumeFileUrl = filePath, // Use ResumeFileUrl for file path
                Title = resume.FileName, // Assuming Title will hold the file name for simplicity
                CreatedAt = DateTime.UtcNow
            };

            _context.Resumes.Add(newResume);
            await _context.SaveChangesAsync();

            return Ok("Resume uploaded successfully");
        }

        [HttpDelete("{resumeId}")]
        public async Task<IActionResult> DeleteResume(int resumeId)
        {
            var resume = await _context.Resumes.FindAsync(resumeId);
            if (resume == null) return NotFound("Resume not found");

            if (System.IO.File.Exists(resume.ResumeFileUrl))
            {
                System.IO.File.Delete(resume.ResumeFileUrl);
            }

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();

            return Ok("Resume deleted successfully");
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetResume(int userId)
        {
            var resume = await _context.Resumes
                .FirstOrDefaultAsync(r => r.UserID == userId);
            if (resume == null) return NotFound("Resume not found");

            try
            {
                var stream = new FileStream(resume.ResumeFileUrl, FileMode.Open);
                return File(stream, "application/octet-stream", resume.Title); // Title used as file name
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error reading the file");
            }
        }
    }
}
