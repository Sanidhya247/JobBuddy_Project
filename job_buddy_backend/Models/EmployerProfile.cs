using job_buddy_backend.Models;


namespace job_buddy_backend.Models
{
    public class EmployerProfile
    {

        public int EmployerProfileID { get; set; }  // Primary Key
        public int UserID { get; set; }  // Foreign Key linking to the User

        public string CompanyName { get; set; } = string.Empty;
        public string CompanyWebsite { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string OfficeAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User Employer { get; set; } = new User();  // Navigation property
    }
}