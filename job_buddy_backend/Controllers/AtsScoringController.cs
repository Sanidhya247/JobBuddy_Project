using job_buddy_backend.Core;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtsScoringController : ControllerBase
    {
        private readonly AtsScoringService _atsScoringService;

        public AtsScoringController(AtsScoringService atsScoringService)
        {
            _atsScoringService = atsScoringService;
        }

        [HttpPost("score")]
        public async Task<IActionResult> GetAtsScore([FromForm] AtsScoringRequest request)
        {
            if (string.IsNullOrEmpty(request.JobDescription) || request.Resume == null)
            {
                return BadRequest("Resume and Job Description cannot be empty.");
            }

            // Save the uploaded resume to a temporary file with the correct extension
            string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + Path.GetExtension(request.Resume.FileName));
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await request.Resume.CopyToAsync(stream);
            }

            try
            {
                var score = _atsScoringService.CalculateAtsScore(tempFilePath, request.JobDescription);
                return Ok(new { Score = score });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                // Clean up the temporary file
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
        }
    }

    public class AtsScoringRequest
    {
        public string JobDescription { get; set; }
        public IFormFile Resume { get; set; }
    }
}
