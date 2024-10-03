using JobBuddyBackend.Models;
using static System.Net.Mime.MediaTypeNames;

namespace job_buddy_backend.Models
{
    public class Resume
    {
        public int ResumeID { get; set; }

        public int UserID { get; set; }

        public string? Title { get; set; }

        public string? ResumeContent { get; set; }

        public string? ResumeFileUrl { get; set; }

        public string? ExperienceSummary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User JobSeeker { get; set; } = new User();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<ATSScore> ATSScores { get; set; } = new List<ATSScore>();
        public ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();

        public Resume()
        {
            Applications = Applications ?? new List<Application>();
            ATSScores = ATSScores ?? new List<ATSScore>();
            ResumeSkills = ResumeSkills ?? new List<ResumeSkill>();
        }
    }
}
