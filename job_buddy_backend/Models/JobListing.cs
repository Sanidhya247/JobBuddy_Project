namespace JobBuddyBackend.Models
{
    public class JobListing
    {
        public int JobID { get; set; }

        public int EmployerID { get; set; }

        public string JobTitle { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? SalaryRange { get; set; }
        public string? JobType { get; set; }

        public string? ExperienceLevel { get; set; }

        public string? Industry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User Employer { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<JobTag> JobTags { get; set; }
    }
}
