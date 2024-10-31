using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(IResumeService resumeService, IWebHostEnvironment environment, ILogger<ResumeController> logger)
        {
            _resumeService = resumeService;
            _environment = environment;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadDto resumeDto, IFormFile resumeFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid resume data."));
                }

                if (resumeFile == null || resumeFile.Length == 0)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("No file provided."));
                }

                // Ensure directory exists
                string uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "resumes");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Generate a unique file name
                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(resumeFile.FileName)}";
                string filePath = Path.Combine(uploadPath, fileName);

                // Save the file to wwwroot/uploads/resumes
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await resumeFile.CopyToAsync(fileStream);
                }

                // Store the relative file URL in the database
                string fileUrl = $"/uploads/resumes/{fileName}";
                resumeDto.ResumeFileUrl = fileUrl;

                var resume = await _resumeService.UploadResumeAsync(resumeDto);

                return Ok(ApiResponse<ResumeDto>.SuccessResponse(resume, "Resume uploaded successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during resume upload.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during resume upload."));
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetResumesByUserId(int userId)
        {
            try
            {
                var resumes = await _resumeService.GetResumesByUserIdAsync(userId);

                if (resumes == null)
                {
                    return NotFound(ApiResponse<string>.FailureResponse("No resumes found."));
                }

                return Ok(ApiResponse<IEnumerable<ResumeDto>>.SuccessResponse(resumes, "Resumes retrieved successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving resumes.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred while retrieving resumes."));
            }
        }
    }
}
