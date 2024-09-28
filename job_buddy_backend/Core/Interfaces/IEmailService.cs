namespace job_buddy_backend.Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirmationAsync(string email, string token, int UserId);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetToken, int UserId);
    }
}
