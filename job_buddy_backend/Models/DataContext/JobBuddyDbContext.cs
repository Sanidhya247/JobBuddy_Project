using job_buddy_backend.Helpers;
using JobBuddyBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace job_buddy_backend.Models.DataContext
{
    public class JobBuddyDbContext : DbContext
    {
        public JobBuddyDbContext(DbContextOptions<JobBuddyDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<JobListing> JobListings { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<ATSScore> ATSScores { get; set; }
        public DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }
        public DbSet<UserEducation> UserEducations { get; set; }
        public DbSet<JobTag> JobTags { get; set; }
        public DbSet<ResumeSkill> ResumeSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppSetting>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<AppSetting>()
                .HasIndex(s => s.SettingKey)
                .IsUnique();

            modelBuilder.Entity<JobListing>()
                .HasOne(j => j.Employer)
                .WithMany()
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasOne(r => r.JobSeeker)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobListing)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeeker)
                .WithMany()
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Resume)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ATSScore>()
                .HasOne(a => a.Resume)
                .WithMany(r => r.ATSScores)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ATSScore>()
                .HasOne(a => a.JobListing)
                .WithMany()
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPhoneNumber>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEducation>()
                .HasOne(ue => ue.User)
                .WithMany()
                .HasForeignKey(ue => ue.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobTag>()
                .HasOne(jt => jt.JobListing)
                .WithMany(j => j.JobTags)
                .HasForeignKey(jt => jt.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            Microsoft.EntityFrameworkCore.Metadata.Builders.ReferenceCollectionBuilder<Resume, ResumeSkill> referenceCollectionBuilder = modelBuilder.Entity<ResumeSkill>()
                .HasOne(rs => rs.Resume)
                .WithMany(r => r.ResumeSkills)
                .HasForeignKey(rs => rs.ResumeID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
