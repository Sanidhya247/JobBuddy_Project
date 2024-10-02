using job_buddy_backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Models.DataContext
{
    public class JobBuddyDbContext : DbContext
    {
        public JobBuddyDbContext(DbContextOptions<JobBuddyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppSetting>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<AppSetting>()
                .HasIndex(s => s.SettingKey)
                .IsUnique();
        }
    }
}
