using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobBuddyBackend.Models
{
    public class JobTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobTagID { get; set; }

        public int JobID { get; set; }

        public string Tag { get; set; } = string.Empty;

        // Navigation property
        public JobListing JobListing { get; set; } = new JobListing();
    }
}
