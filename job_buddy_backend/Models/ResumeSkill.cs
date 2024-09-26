namespace job_buddy_backend.Models
{
    public class ResumeSkill
    {
        public int ResumeSkillID { get; set; }

        public int ResumeID { get; set; }

        public string Skill { get; set; } = string.Empty;


        public Resume Resume { get; set; } = new Resume();
    }
}
