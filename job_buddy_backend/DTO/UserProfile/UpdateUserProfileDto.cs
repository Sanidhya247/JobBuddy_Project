namespace job_buddy_backend.DTO.UserProfile
{
    public class UpdateUserProfileDto
    {
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? PhoneNumber { get; set; } 
    }
}
