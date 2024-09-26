﻿namespace job_buddy_backend.DTO
{
    public class RegisterUserDto
    {
        public string FullName { get; set; } = string.Empty;  

        public string Email { get; set; } = string.Empty;  

        public string Password { get; set; } = string.Empty;  

        public string Role { get; set; } = string.Empty;  

        public string? PhoneNumber { get; set; }  

        public string? Address { get; set; }  

        public string? LinkedInUrl { get; set; }  

    }
}
