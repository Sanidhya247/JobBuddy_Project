using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Helpers;
using job_buddy_backend.Models.UserModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace job_buddy_backend.Core
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationService _configurationService;

        public JwtService(IConfiguration configuration, IConfigurationService configurationService)
        {
            _configuration = configuration;
            _configurationService = configurationService;
        }

        public async Task<string> GenerateToken(User user)
        {
            // Create claims based on the user's information
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserID", user.UserID.ToString())
            };

            // Get the key and credentials from the configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(await _configurationService.GetSettingAsync("JwtKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime currentTime = DateTime.UtcNow;
            DateTime tokenExpiration = currentTime.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]));
            var expiryMinutes = Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"]);
            var expiration = currentTime.AddMinutes(expiryMinutes);
            Console.WriteLine($"Token generated at: {currentTime}");
            Console.WriteLine($"Token expires at: {tokenExpiration}");
            // Define the token's expiration and other properties
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            // Return the token in string format
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
