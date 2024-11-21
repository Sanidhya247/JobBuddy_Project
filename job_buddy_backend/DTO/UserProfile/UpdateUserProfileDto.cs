namespace job_buddy_backend.DTO.UserProfile
{
    public class UpdateUserProfileDto
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? Headline { get; set; }
        public string? About { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<UserPhoneNumberDto>? PhoneNumbers { get; set; } = new List<UserPhoneNumberDto>();
        public ICollection<UserEducationDto>? Educations { get; set; } = new List<UserEducationDto>();
        public ICollection<UserExperienceDto>? Experiences { get; set; } = new List<UserExperienceDto>();
        public ICollection<UserCertificationDto>? Certifications { get; set; } = new List<UserCertificationDto>();
        public ICollection<UserProjectDto>? Projects { get; set; } = new List<UserProjectDto>();
    }
}
