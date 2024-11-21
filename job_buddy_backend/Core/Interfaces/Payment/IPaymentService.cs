using job_buddy_backend.DTO.Payment;
using job_buddy_backend.Models;

namespace job_buddy_backend.Core.Interfaces.Payment
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResponseDto>> CreatePaymentIntentAsync(PaymentRequestDto paymentRequest);
    }
}
