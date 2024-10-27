namespace job_buddy_backend.DTO.UserProfile
{
    public class UserProfileDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsProfileComplete { get; set; }
        public double ProfileCompletenessPercentage { get; set; }
        public ICollection<UserEducationDto> Educations { get; set; }
        public ICollection<UserPhoneNumberDto> PhoneNumbers { get; set; }
    }
}
