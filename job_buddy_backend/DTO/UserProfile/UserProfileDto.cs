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
        public string Headline { get; set; } = string.Empty;
        public string About { get; set; } = string.Empty;
        public string? CoverPhotoUrl { get; set; }
        public bool IsPremium { get; set; }
        public ICollection<UserEducationDto> Educations { get; set; } = new List<UserEducationDto>();
        public ICollection<UserPhoneNumberDto> PhoneNumbers { get; set; } = new List<UserPhoneNumberDto>();
        public ICollection<UserExperienceDto> Experiences { get; set; } = new List<UserExperienceDto>();
        public ICollection<UserCertificationDto> Certifications { get; set; } = new List<UserCertificationDto>();
        public ICollection<UserProjectDto> Projects { get; set; } = new List<UserProjectDto>();
    }
}
