using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using Microsoft.Extensions.Logging;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models.UserModel;

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
                    PasswordHash = HashPassword(registerDto.Password),
                    Role = registerDto.Role,
                    IsEmailVerified = true, 
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmationToken = GenerateConfirmationToken()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); 
         
                await transaction.CommitAsync(); 
                return user;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred during user registration.");
                throw;
            }
        }

        
        public async Task<User> LoginAsync(LoginUserDto loginDto)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Invalid credentials! Please try again.");
                }

                if (!user.IsActive)
                {
                    throw new UnauthorizedAccessException("Your account is deactivated, Please contact admin!");
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
                _logger.LogError(ex, "An error occurred during login. Please contact Admin.");
                throw;
            }
        }

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

        public async Task<bool> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null || user.EmailConfirmationToken != token)
            {
                return false;
            }

            user.IsEmailVerified = true;
            user.EmailConfirmationToken = null; 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null || user.PasswordResetToken != token)
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword); 
            user.PasswordResetToken = null; 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user).Result; 
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword); 
        }

        private string GenerateConfirmationToken()
        {
            return Guid.NewGuid().ToString(); 
        }

        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString(); 
        }

        //Get User by user id
        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }
            return user;
        }
    }
}
