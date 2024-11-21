using JobBuddyBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using job_buddy_backend.Models.UserModel;
using System.Text.Json.Serialization;


namespace job_buddy_backend.Models
{
    public class Application
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public DateTime StartDate { get; set; }
        public string Linkedin { get; set; }
        public string Status { get; set; } = "Submitted";
        public string? CoverLetter { get; set; }
        public bool FollowUpReminder { get; set; } = false;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; }
        public int JobID { get; set; }
        public int ResumeID { get; set; }
        [ForeignKey("JobID")]
        [JsonIgnore] 
        public JobListing? JobListing { get; set; }

        [ForeignKey("UserID")]
        [JsonIgnore]
        public User? JobSeeker { get; set; }

        [ForeignKey("ResumeID")]
        [JsonIgnore]
        public Resume? Resume { get; set; }
    }
}
