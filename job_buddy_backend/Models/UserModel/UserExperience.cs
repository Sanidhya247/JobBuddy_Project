using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.UserModel
{
    public class UserExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserExperienceID { get; set; }

        public int UserID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }

        public User User { get; set; } = new User();
    }

}
