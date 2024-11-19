using job_buddy_backend.Models.UserModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using JobBuddyBackend.Models;

namespace job_buddy_backend.Models.ChatModel
{
    public class Chat
    {
        [Key]
        public int ChatID { get; set; }

        [ForeignKey("JobSeeker")]
        public int JobSeekerID { get; set; }
        public User JobSeeker { get; set; }

        [ForeignKey("Employer")]
        public int EmployerID { get; set; }
        public User Employer { get; set; }

        public bool IsActive { get; set; } = true;

        public int? JobID { get; set; }  
        public JobListing Job { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

}
