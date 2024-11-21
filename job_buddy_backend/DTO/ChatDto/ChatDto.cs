namespace job_buddy_backend.DTO.ChatDto
{
    public class ChatDto
    {
        public int JobSeekerID { get; set; }
        public int EmployerID { get; set; }
        public bool IsActive { get; set; } = true;
        public int? JobID { get; set; }  
        public int ChatID { get; set; }
        public string UserName { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public string LastMessage {  get; set; }
    }

}
