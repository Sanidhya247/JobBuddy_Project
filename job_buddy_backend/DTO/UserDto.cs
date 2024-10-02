namespace job_buddy_backend.DTO
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsEmailVerified { get; set; }
    }
}
