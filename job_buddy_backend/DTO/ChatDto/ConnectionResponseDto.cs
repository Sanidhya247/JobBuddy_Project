using job_buddy_backend.Models.ChatModel;

namespace job_buddy_backend.DTO.ChatDto
{
    public class ConnectionResponseDto
    {
        public int ConnectionID { get; set; }
        public int RequestorID { get; set; }
        public int RequesteeID { get; set; }
        public ConnectionStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
    }
}
