using job_buddy_backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobBuddyBackend.Models
{
    public class ATSScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ATSScoreID { get; set; }

        public int ResumeID { get; set; }

        public int JobID { get; set; }

        public decimal Score { get; set; }

        public string? ATSFeedback { get; set; }

        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Resume Resume { get; set; } 
        public JobListing JobListing { get; set; } 
    }
}
