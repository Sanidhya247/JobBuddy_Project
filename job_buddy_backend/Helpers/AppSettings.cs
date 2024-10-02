using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace job_buddy_backend.Helpers
{
    public class AppSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string SettingKey { get; set; }
        [Required]
        public string SettingValue { get; set; }
    }
}
