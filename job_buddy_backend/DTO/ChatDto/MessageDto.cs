namespace job_buddy_backend.DTO.ChatDto
{
    public class MessageDto
    {
        public int ChatID { get; set; }
        public int SenderID { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}
