using job_buddy_backend.Helpers;
using job_buddy_backend.Models.UserModel;
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
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public DbSet<UserCertification> UserCertifications { get; set; }
        public DbSet<UserExperience> UserExperiences { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppSetting Configuration
            modelBuilder.Entity<AppSetting>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<AppSetting>()
                .HasIndex(s => s.SettingKey)
                .IsUnique();

            // JobListing Configuration
            modelBuilder.Entity<JobListing>()
                .Property(j => j.PayRatePerYear)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<JobListing>()
                .Property(j => j.PayRatePerHour)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<JobListing>()
                .HasOne(j => j.Employer)
                .WithMany(u => u.JobListings)
                .HasForeignKey(j => j.EmployerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Resume Configuration
            modelBuilder.Entity<Resume>()
                .HasOne(r => r.JobSeeker)
                .WithMany(u => u.Resumes)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.Applications)
                .WithOne(a => a.Resume)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.ATSScores)
                .WithOne(s => s.Resume)
                .HasForeignKey(s => s.ResumeID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Resume>()
                .HasMany(r => r.ResumeSkills)
                .WithOne(rs => rs.Resume)
                .HasForeignKey(rs => rs.ResumeID)
                .OnDelete(DeleteBehavior.Cascade);

            // Application Configuration
            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobListing)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeeker)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserID)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Application>()
                .HasOne(a => a.Resume)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.Restrict);

            // ATSScore Configuration
            modelBuilder.Entity<ATSScore>()
                .Property(a => a.Score)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ATSScore>()
                .HasOne(a => a.Resume)
                .WithMany(r => r.ATSScores)
                .HasForeignKey(a => a.ResumeID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ATSScore>()
                .HasOne(a => a.JobListing)
                .WithMany()
                .HasForeignKey(a => a.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            // UserPhoneNumber Configuration
            modelBuilder.Entity<UserPhoneNumber>()
                .HasOne(up => up.User)
                .WithMany(u => u.PhoneNumbers)
                .HasForeignKey(up => up.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // UserEducation Configuration
            modelBuilder.Entity<UserEducation>()
                .HasOne(ue => ue.User)
                .WithMany(u => u.Educations)
                .HasForeignKey(ue => ue.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // JobTag Configuration
            modelBuilder.Entity<JobTag>()
                .HasOne(jt => jt.JobListing)
                .WithMany(j => j.JobTags)
                .HasForeignKey(jt => jt.JobID)
                .OnDelete(DeleteBehavior.Cascade);

            // ResumeSkill Configuration
            modelBuilder.Entity<ResumeSkill>()
                .HasOne(rs => rs.Resume)
                .WithMany(r => r.ResumeSkills)
                .HasForeignKey(rs => rs.ResumeID)
                .OnDelete(DeleteBehavior.Cascade);

            // EmployerProfile Configuration
            modelBuilder.Entity<EmployerProfile>()
                .HasOne(ep => ep.Employer)
                .WithMany()
                .HasForeignKey(ep => ep.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
