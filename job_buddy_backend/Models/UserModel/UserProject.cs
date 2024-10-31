using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.UserModel
{
    public class UserProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserProjectID { get; set; }

        public int UserID { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Link { get; set; }

        public User User { get; set; } = new User();
    }
}
