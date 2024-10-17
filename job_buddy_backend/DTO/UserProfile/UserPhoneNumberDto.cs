namespace job_buddy_backend.DTO.UserProfile
{
    public class UserPhoneNumberDto
    {
        public int UserPhoneNumberID { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Type { get; set; }
    }
}
