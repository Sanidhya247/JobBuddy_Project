using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.UserModel
{
    public class UserCertification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserCertificationID { get; set; }

        public int UserID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public string? CredentialUrl { get; set; }

        public User User { get; set; } = new User();
    }
}
