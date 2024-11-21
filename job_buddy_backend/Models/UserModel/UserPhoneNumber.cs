using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Models.UserModel
{
    public class UserPhoneNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserPhoneNumberID { get; set; }

        public int UserID { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Type { get; set; }


        public User User { get; set; } = new User();
    }
}
