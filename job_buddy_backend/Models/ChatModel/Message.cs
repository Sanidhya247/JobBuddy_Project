using job_buddy_backend.Models.UserModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.ChatModel
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("Chat")]
        public int ChatID { get; set; }
        public Chat Chat { get; set; }

        [ForeignKey("Sender")]
        public int SenderID { get; set; }
        public User Sender { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead {  get; set; }
    }
}
