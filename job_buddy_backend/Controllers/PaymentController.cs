using job_buddy_backend.Core.Interfaces.Payment;
using job_buddy_backend.DTO.Payment;
using job_buddy_backend.Models.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly JobBuddyDbContext _context;

        public PaymentController(IPaymentService paymentService, JobBuddyDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost("create-payment-intent")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequestDto paymentRequest)
        {
            var result = await _paymentService.CreatePaymentIntentAsync(paymentRequest);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update-premium-status/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdatePremiumStatus(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return NotFound("User not found.");

                user.IsPremium = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "User upgraded to premium successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
