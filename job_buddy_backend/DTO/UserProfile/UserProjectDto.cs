namespace job_buddy_backend.DTO.UserProfile
{
    public class UserProjectDto
    {
        public int UserProjectID { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
