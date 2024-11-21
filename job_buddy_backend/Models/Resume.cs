using JobBuddyBackend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using job_buddy_backend.Models.UserModel;

namespace job_buddy_backend.Models
{
    public class Resume
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResumeID { get; set; }

        public string? Title { get; set; }

        public string? ResumeContent { get; set; }

        public string? ResumeFileUrl { get; set; }

        public string? ExperienceSummary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User? JobSeeker { get; set; } 
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<ATSScore> ATSScores { get; set; } = new List<ATSScore>();
        public ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

    }
}
