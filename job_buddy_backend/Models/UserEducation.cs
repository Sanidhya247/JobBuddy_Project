namespace job_buddy_backend.Models
{
    public class UserEducation
    {
        public int UserEducationID { get; set; }

        public int UserID { get; set; }

        public string Degree { get; set; } = string.Empty;

        public string Institution { get; set; } = string.Empty;

        public DateTime? GraduationDate { get; set; }


        public User User { get; set; } = new User();
    }
}
