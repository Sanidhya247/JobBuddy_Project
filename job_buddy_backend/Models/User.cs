namespace job_buddy_backend.Models
{
    //This is the model for user class
    public class User
    {
        public int UserID { get; set; }  // Primary Key

        public string FullName { get; set; } = string.Empty;  

        public string Email { get; set; } = string.Empty;  

        public string PasswordHash { get; set; } = string.Empty;  

        public string Role { get; set; } = string.Empty;  //"Job Seeker" or "Employer" value

        public string? Address { get; set; }  

        public string? ProfilePictureUrl { get; set; }  

        public DateTime? DateOfBirth { get; set; }  

        public string? Nationality { get; set; }  

        public string? LinkedInUrl { get; set; }  

        public bool IsProfileComplete { get; set; } = false;

        public bool IsEmailVerified { get; set; } = false;

        public DateTime? LastLoginAt { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? EmailConfirmationToken { get; set; }  

        public string? PasswordResetToken { get; set; }

        ////Navigation properties
        //public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
        //public ICollection<JobListing> JobListings { get; set; } = new List<JobListing>();
        //public ICollection<UserPhoneNumber> PhoneNumbers { get; set; } = new List<UserPhoneNumber>();
        //public ICollection<UserEducation> Educations { get; set; } = new List<UserEducation>();

        //// Makes sure navigation properties are initialized
        //public User()
        //{
        //    Resumes = Resumes ?? new List<Resume>();
        //    JobListings = JobListings ?? new List<JobListing>();
        //    PhoneNumbers = PhoneNumbers ?? new List<UserPhoneNumber>();
        //    Educations = Educations ?? new List<UserEducation>();
        //}
    }
}
