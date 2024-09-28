using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.Extensions.Logging;
using job_buddy_backend.Models.DataContext;

namespace job_buddy_backend.Core
{
    public class AuthService : IAuthService
    {
        private readonly JobBuddyDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthService(JobBuddyDbContext context, ILogger<AuthService> logger, IJwtService jwtService, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        // Register new user with a single transaction
        public async Task<User> RegisterAsync(RegisterUserDto registerDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("User with this email already exists.");
                }

                var user = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    PasswordHash = HashPassword(registerDto.Password), // Hash password before saving
                    Role = registerDto.Role,
                    IsEmailVerified = false, // Set as false until email confirmation
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmationToken = GenerateConfirmationToken() // Generate confirmation token
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Save user to DB

                var emailSent = await _emailService.SendEmailConfirmationAsync(user.Email, user.EmailConfirmationToken, user.UserID);

                if (!emailSent)
                {
                    await transaction.RollbackAsync(); // Rollback if email sending fails
                    throw new InvalidOperationException("Error sending confirmation email. Registration rolled back.");
                }

                await transaction.CommitAsync(); // Commit transaction if everything is successful
                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction on error
                _logger.LogError(ex, "An error occurred during user registration.");
                throw;
            }
        }

        // Login user with error handling
        public async Task<User> LoginAsync(LoginUserDto loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid credentials.");
                }

                if (!user.IsEmailVerified)
                {
                    throw new UnauthorizedAccessException("Please confirm your email before logging in.");
                }

                return user;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Unauthorized login attempt for user with email {loginDto.Email}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                throw;
            }
        }

        // Generate email confirmation token (direct DB operation)
        public async Task<string> GenerateEmailConfirmationTokenAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            user.EmailConfirmationToken = GenerateConfirmationToken();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.EmailConfirmationToken;
        }

        // Generate password reset token (direct DB operation)
        public async Task<string> GeneratePasswordResetTokenAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var token = GenerateResetToken();
            user.PasswordResetToken = token;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return token;
        }

        // Get user by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                _logger.LogWarning($"User with email {email} not found.");
                throw new InvalidOperationException("User not found.");
            }
            return user;
        }

        // Confirm email without transaction
        public async Task<bool> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null || user.EmailConfirmationToken != token)
            {
                return false;
            }

            user.IsEmailVerified = true;
            user.EmailConfirmationToken = null; // Clear the token once confirmed
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // Reset password without transaction
        public async Task<bool> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null || user.PasswordResetToken != token)
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword); // Hash new password
            user.PasswordResetToken = null; // Clear reset token
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // Generate JWT Token for the logged-in user
        public string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user); // Use JwtService to create the token
        }

        // Hash the user's password
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); // Using BCrypt for hashing
        }

        // Verify the user's password
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword); // Verify hashed password
        }

        // Generate a token for email confirmation
        private string GenerateConfirmationToken()
        {
            return Guid.NewGuid().ToString(); // Use GUID here; can be replaced by JWT if needed
        }

        // Generate a token for password reset
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString(); // Use GUID here; can be replaced by JWT if needed
        }
    }
}
