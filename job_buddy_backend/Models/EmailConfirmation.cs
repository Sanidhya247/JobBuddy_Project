namespace job_buddy_backend.Models
{
    public class EmailConfirmation
    {
        public int ConfirmationID { get; set; }

        public int UserID
        {
            get; set;
        }

        public string ConfirmationToken { get; set; } = string.Empty;

        public bool IsConfirmed { get; set; } = false;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddDays(1);


        public User User { get; set; } = new User();
    }
}
