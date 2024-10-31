namespace job_buddy_backend.DTO
{
    public class ResumeDto
    {
        public int ResumeID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public string? ResumeContent { get; set; }
        public string? ResumeFileUrl { get; set; }
        public string? ExperienceSummary { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
