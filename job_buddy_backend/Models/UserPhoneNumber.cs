namespace job_buddy_backend.Models
{
    public class UserPhoneNumber
    {
        public int UserPhoneNumberID { get; set; }

        public int UserID { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Type { get; set; }


        public User User { get; set; } = new User();
    }
}
