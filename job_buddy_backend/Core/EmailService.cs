using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Models;

namespace job_buddy_backend.Core
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailConfirmationAsync(string email, string token, int userId)
        {
            try
            {
                var apiKey = _configuration["EmailSettings:MailgunApiKey"];
                var domain = _configuration["EmailSettings:MailgunDomain"];
                var baseUrl = _configuration["EmailSettings:MailgunBaseUrl"];
                var fromEmail = _configuration["EmailSettings:SenderEmail"];
                var fromName = _configuration["EmailSettings:SenderName"];
                var confirmationUrl = $"{_configuration["AppSettings:BaseUrl"]}/api/auth/confirm-email?token={token}&userId={userId}";
                

                // Initialize RestClient with BaseUrl
                var client = new RestClient(new RestClientOptions(baseUrl)
                {
                    Authenticator = new HttpBasicAuthenticator("api", apiKey) // Set Authenticator in constructor
                });

                var request = new RestRequest($"{domain}/messages", Method.Post);
                request.AddParameter("from", $"{fromName} <{fromEmail}>");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Please confirm your email: Job Buddy");
                request.AddParameter("text", $"Please confirm your email by clicking on the following link: {confirmationUrl}");

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    _logger.LogError($"Failed to send email confirmation to {email}. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending email confirmation to {email}");
                return false;
            }
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetToken, int userId)
        {
            try
            {
                var apiKey = _configuration["EmailSettings:MailgunApiKey"];
                var domain = _configuration["EmailSettings:MailgunDomain"];
                var baseUrl = _configuration["EmailSettings:MailgunBaseUrl"];
                var fromEmail = _configuration["EmailSettings:SenderEmail"];
                var fromName = _configuration["EmailSettings:SenderName"];
                var resetUrl = $"{_configuration["AppSettings:BaseUrl"]}/api/auth/reset-password?token={resetToken}&userId={userId}";


                // Initialize RestClient with BaseUrl
                var client = new RestClient(new RestClientOptions(baseUrl)
                {
                    Authenticator = new HttpBasicAuthenticator("api", apiKey) // Set Authenticator in constructor
                });

                var request = new RestRequest($"{domain}/messages", Method.Post);
                request.AddParameter("from", $"{fromName} <{fromEmail}>");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Password Reset Request");
                request.AddParameter("text", $"You can reset your password by clicking on the following link: {resetUrl}");

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    _logger.LogError($"Failed to send password reset email to {email}. Status: {response.StatusCode}, Error: {response.ErrorMessage}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending password reset email to {email}");
                return false;
            }
        }
    }
}
