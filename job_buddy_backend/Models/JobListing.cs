using job_buddy_backend.Models;
using job_buddy_backend.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBuddyBackend.Models
{
    public class JobListing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobID { get; set; }
        public int EmployerID { get; set; }

        public string JobTitle { get; set; } = string.Empty;
        public string ShortJobDescription { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;

        public string Province { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string? SalaryRange { get; set; }
        public decimal? PayRatePerYear { get; set; }
        public decimal? PayRatePerHour { get; set; }

        public string? JobType { get; set; }
        public string? WorkType { get; set; } = "Hybrid";  // Hybrid, Remote, In-Person

        public string? ExperienceLevel { get; set; }
        public string? Industry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User Employer { get; set; } = new User();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<JobTag> JobTags { get; set; } = new List<JobTag>();
    }
}
