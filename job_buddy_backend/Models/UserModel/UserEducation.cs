using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.UserModel
{
    public class UserEducation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserEducationID { get; set; }

        public int UserID { get; set; }

        public string Degree { get; set; } = string.Empty;

        public string Institution { get; set; } = string.Empty;

        public DateTime? GraduationDate { get; set; }


        public User User { get; set; } = new User();
    }
}
