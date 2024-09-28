namespace job_buddy_backend.DTO
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;


        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
