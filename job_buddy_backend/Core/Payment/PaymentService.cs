using job_buddy_backend.Core.Interfaces.Payment;
using job_buddy_backend.DTO.Payment;
using job_buddy_backend.Helpers;
using job_buddy_backend.Models;
using Stripe;

/*Payment stripe references - https://docs.stripe.com/api/payment_intents/create?lang=dotnet */
namespace job_buddy_backend.Core.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfigurationService _configurationService;

        public PaymentService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<ApiResponse<PaymentResponseDto>> CreatePaymentIntentAsync(PaymentRequestDto paymentRequest)
        {
            try
            {
                StripeConfiguration.ApiKey = await _configurationService.GetSettingAsync("PaymentSecretKey");

                var options = new PaymentIntentCreateOptions
                {
                    Amount = paymentRequest.Amount,
                    Currency = "cad",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                var response = new PaymentResponseDto
                {
                    ClientSecret = paymentIntent.ClientSecret
                };

                return ApiResponse<PaymentResponseDto>.SuccessResponse(response, "PaymentIntent created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaymentResponseDto>.FailureResponse($"Error creating PaymentIntent: {ex.Message}");
            }
        }
    }
}
