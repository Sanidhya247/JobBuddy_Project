using JobBuddyBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace job_buddy_backend.Models
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationID { get; set; }

        public int JobID { get; set; }

        public int UserID { get; set; }

        public int ResumeID { get; set; }

        public string Status { get; set; } = "Submitted";

        public string? CoverLetter { get; set; }

        public string? AppliedVia { get; set; }

        public bool FollowUpReminder { get; set; } = false;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;


        public JobListing JobListing { get; set; } = new JobListing();
        public User JobSeeker { get; set; } = new User();
        public Resume Resume { get; set; } = new Resume();
    }
}
