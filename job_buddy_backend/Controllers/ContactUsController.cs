using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using job_buddy_backend.Core;
using job_buddy_backend.DTO;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactUsController : ControllerBase
    {
        private readonly ContactUsService _contactUsService;

        public ContactUsController(ContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitContactRequest([FromBody] ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var result = await _contactUsService.SubmitContactRequest(contactUsDto);
            return result ? Ok("Contact request submitted successfully.") : StatusCode(500, "Failed to submit request.");
        }
    }
}
