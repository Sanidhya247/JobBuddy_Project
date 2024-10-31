using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.DTO
{
    public class ResumeUploadDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string Title { get; set; }

        public string? ResumeContent { get; set; }

        public string? ResumeFileUrl { get; set; }
        public string? ExperienceSummary { get; internal set; }
    }
}
