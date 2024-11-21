namespace job_buddy_backend.Models.ChatModel
{
    public class Connection
    {
        public int ConnectionID { get; set; }
        public int RequestorID { get; set; }  // User who sends the request
        public int RequesteeID { get; set; }  // User who receives the request
        public ConnectionStatus Status { get; set; } = ConnectionStatus.Pending;
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcceptedAt { get; set; }
        public int? JobID { get; set; }  
    }


    public enum ConnectionStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blocked
    }

}
