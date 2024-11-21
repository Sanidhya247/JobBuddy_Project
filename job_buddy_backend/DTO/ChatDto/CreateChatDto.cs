namespace job_buddy_backend.DTO.ChatDto
{
    public class CreateChatDto
    {
        public int JobSeekerID { get; set; }
        public int EmployerID { get; set; }
        public int? JobID { get; set; }
    }
}
