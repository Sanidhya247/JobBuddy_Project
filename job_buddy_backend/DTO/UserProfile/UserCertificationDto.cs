namespace job_buddy_backend.DTO.UserProfile
{
    public class UserCertificationDto
    {
        public int UserCertificationID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public string CredentialUrl { get; set; } = string.Empty;
    }
}
