namespace job_buddy_backend.DTO.UserProfile
{
    public class UserEducationDto
    {
        public int UserEducationID { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public DateTime? GraduationDate { get; set; }
    }
}
