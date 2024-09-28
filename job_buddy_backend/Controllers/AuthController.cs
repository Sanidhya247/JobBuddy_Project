using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace job_buddy_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IEmailService emailService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _emailService = emailService;
            _logger = logger;
        }

        // Register user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid registration data."));
                }

                var user = await _authService.RegisterAsync(registerDto);

                //var confirmationToken = await _authService.GenerateEmailConfirmationTokenAsync(user.UserID);
                //var emailSent = await _emailService.SendEmailConfirmationAsync(user.Email, confirmationToken, user.UserID);

                if (user == null)
                {
                    return StatusCode(500, ApiResponse<string>.FailureResponse("Unable to register user! Please contact admin"));
                }

                _logger.LogInformation($"User {user.Email} registered successfully.");
                return Ok(ApiResponse<string>.SuccessResponse("Registration successful. Please check your email to confirm your account."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during registration."));
            }
        }

        // Login user
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid login data."));
                }

                var user = await _authService.LoginAsync(loginDto);

                if (!user.IsEmailVerified)
                {
                    return Unauthorized(ApiResponse<string>.FailureResponse("Please confirm your email before logging in."));
                }

                var token = _authService.GenerateJwtToken(user);
                return Ok(ApiResponse<string>.SuccessResponse(token, "Login successful."));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Invalid login attempt for {loginDto.Email}.");
                return Unauthorized(ApiResponse<string>.FailureResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during login."));
            }
        }

        // Forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid forgot password request."));
                }

                var user = await _authService.GetUserByEmailAsync(forgotPasswordDto.Email);

                if (user == null)
                {
                    return NotFound(ApiResponse<string>.FailureResponse("User not found."));
                }

                var resetToken = await _authService.GeneratePasswordResetTokenAsync(user.UserID);
                var emailSent = await _emailService.SendPasswordResetEmailAsync(user.Email, resetToken, user.UserID);

                if (!emailSent)
                {
                    return StatusCode(500, ApiResponse<string>.FailureResponse("Error sending password reset email."));
                }

                return Ok(ApiResponse<string>.SuccessResponse("Password reset email sent successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during forgot password.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during forgot password."));
            }
        }

        // Confirm email endpoint
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || userId <= 0)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid email confirmation request."));
                }

                var isConfirmed = await _authService.ConfirmEmailAsync(userId, token);

                if (!isConfirmed)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid or expired confirmation token."));
                }

                _logger.LogInformation($"User {userId} confirmed email successfully.");
                return Ok(ApiResponse<string>.SuccessResponse("Email confirmed successfully."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during email confirmation.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during email confirmation."));
            }
        }

        // Reset Password Endpoint
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string token, [FromQuery] int userId, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || userId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid password reset request."));
                }

                // Check if the new password meets criteria
                if (string.IsNullOrWhiteSpace(resetPasswordDto.NewPassword) || resetPasswordDto.NewPassword.Length < 6)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Password must be at least 6 characters long."));
                }

                var isResetSuccessful = await _authService.ResetPasswordAsync(userId, token, resetPasswordDto.NewPassword);

                if (!isResetSuccessful)
                {
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid or expired reset token."));
                }

                _logger.LogInformation($"User {userId} reset password successfully.");
                return Ok(ApiResponse<string>.SuccessResponse("Password reset successful."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during password reset.");
                return StatusCode(500, ApiResponse<string>.FailureResponse("An error occurred during password reset."));
            }
        }

    }
}
