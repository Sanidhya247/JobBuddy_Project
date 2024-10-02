namespace JobBuddyBackend.Models
{
    public class JobTag
    {
        public int JobTagID { get; set; }

        public int JobID { get; set; }

        public string Tag { get; set; } = string.Empty;

        // Navigation property
        public JobListing JobListing { get; set; }
    }
}
