namespace job_buddy_backend.DTO.Payment
{
    public class PaymentRequestDto
    {
        public int Amount { get; set; } 
    }

    public class PaymentResponseDto
    {
        public string ClientSecret { get; set; }
    }
}
