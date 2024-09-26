using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Models.DataContext
{
    public class JobBuddyDbContext : DbContext
    {
        public JobBuddyDbContext(DbContextOptions<JobBuddyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
