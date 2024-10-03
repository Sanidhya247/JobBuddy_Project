﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using job_buddy_backend.Models.DataContext;

#nullable disable

namespace job_buddy_backend.Migrations
{
    [DbContext(typeof(JobBuddyDbContext))]
    partial class JobBuddyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobBuddyBackend.Models.ATSScore", b =>
                {
                    b.Property<int>("ATSScoreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ATSScoreID"));

                    b.Property<string>("ATSFeedback")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CheckedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("JobID")
                        .HasColumnType("int");

                    b.Property<int>("ResumeID")
                        .HasColumnType("int");

                    b.Property<decimal>("Score")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ATSScoreID");

                    b.HasIndex("JobID");

                    b.HasIndex("ResumeID");

                    b.ToTable("ATSScores");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.JobListing", b =>
                {
                    b.Property<int>("JobID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobID"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployerID")
                        .HasColumnType("int");

                    b.Property<string>("ExperienceLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("JobType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PayRatePerHour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PayRatePerYear")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SalaryRange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortJobDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("WorkType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobID");

                    b.HasIndex("EmployerID");

                    b.HasIndex("UserID");

                    b.ToTable("JobListings");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.JobTag", b =>
                {
                    b.Property<int>("JobTagID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobTagID"));

                    b.Property<int>("JobID")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("JobTagID");

                    b.HasIndex("JobID");

                    b.ToTable("JobTags");
                });

            modelBuilder.Entity("job_buddy_backend.Helpers.AppSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("SettingKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SettingValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SettingKey")
                        .IsUnique();

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("job_buddy_backend.Models.Application", b =>
                {
                    b.Property<int>("ApplicationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationID"));

                    b.Property<string>("AppliedVia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverLetter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FollowUpReminder")
                        .HasColumnType("bit");

                    b.Property<int>("JobID")
                        .HasColumnType("int");

                    b.Property<int>("ResumeID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ApplicationID");

                    b.HasIndex("JobID");

                    b.HasIndex("ResumeID");

                    b.HasIndex("UserID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("job_buddy_backend.Models.EmployerProfile", b =>
                {
                    b.Property<int>("EmployerProfileID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployerProfileID"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyWebsite")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OfficeAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployerProfileID");

                    b.HasIndex("UserID");

                    b.ToTable("EmployerProfiles");
                });

            modelBuilder.Entity("job_buddy_backend.Models.Resume", b =>
                {
                    b.Property<int>("ResumeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResumeID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExperienceSummary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResumeContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResumeFileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID1")
                        .HasColumnType("int");

                    b.HasKey("ResumeID");

                    b.HasIndex("UserID");

                    b.HasIndex("UserID1");

                    b.ToTable("Resumes");
                });

            modelBuilder.Entity("job_buddy_backend.Models.ResumeSkill", b =>
                {
                    b.Property<int>("ResumeSkillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ResumeSkillID"));

                    b.Property<int>("ResumeID")
                        .HasColumnType("int");

                    b.Property<string>("Skill")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ResumeSkillID");

                    b.HasIndex("ResumeID");

                    b.ToTable("ResumeSkills");
                });

            modelBuilder.Entity("job_buddy_backend.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailConfirmationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsProfileComplete")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LinkedInUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nationality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("job_buddy_backend.Models.UserEducation", b =>
                {
                    b.Property<int>("UserEducationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserEducationID"));

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("GraduationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID1")
                        .HasColumnType("int");

                    b.HasKey("UserEducationID");

                    b.HasIndex("UserID");

                    b.HasIndex("UserID1");

                    b.ToTable("UserEducations");
                });

            modelBuilder.Entity("job_buddy_backend.Models.UserPhoneNumber", b =>
                {
                    b.Property<int>("UserPhoneNumberID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPhoneNumberID"));

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("UserID1")
                        .HasColumnType("int");

                    b.HasKey("UserPhoneNumberID");

                    b.HasIndex("UserID");

                    b.HasIndex("UserID1");

                    b.ToTable("UserPhoneNumbers");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.ATSScore", b =>
                {
                    b.HasOne("JobBuddyBackend.Models.JobListing", "JobListing")
                        .WithMany()
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.Resume", "Resume")
                        .WithMany("ATSScores")
                        .HasForeignKey("ResumeID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("JobListing");

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.JobListing", b =>
                {
                    b.HasOne("job_buddy_backend.Models.User", "Employer")
                        .WithMany()
                        .HasForeignKey("EmployerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.User", null)
                        .WithMany("JobListings")
                        .HasForeignKey("UserID");

                    b.Navigation("Employer");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.JobTag", b =>
                {
                    b.HasOne("JobBuddyBackend.Models.JobListing", "JobListing")
                        .WithMany("JobTags")
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobListing");
                });

            modelBuilder.Entity("job_buddy_backend.Models.Application", b =>
                {
                    b.HasOne("JobBuddyBackend.Models.JobListing", "JobListing")
                        .WithMany("Applications")
                        .HasForeignKey("JobID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.Resume", "Resume")
                        .WithMany("Applications")
                        .HasForeignKey("ResumeID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.User", "JobSeeker")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("JobListing");

                    b.Navigation("JobSeeker");

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("job_buddy_backend.Models.EmployerProfile", b =>
                {
                    b.HasOne("job_buddy_backend.Models.User", "Employer")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employer");
                });

            modelBuilder.Entity("job_buddy_backend.Models.Resume", b =>
                {
                    b.HasOne("job_buddy_backend.Models.User", "JobSeeker")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.User", null)
                        .WithMany("Resumes")
                        .HasForeignKey("UserID1");

                    b.Navigation("JobSeeker");
                });

            modelBuilder.Entity("job_buddy_backend.Models.ResumeSkill", b =>
                {
                    b.HasOne("job_buddy_backend.Models.Resume", "Resume")
                        .WithMany("ResumeSkills")
                        .HasForeignKey("ResumeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Resume");
                });

            modelBuilder.Entity("job_buddy_backend.Models.UserEducation", b =>
                {
                    b.HasOne("job_buddy_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.User", null)
                        .WithMany("Educations")
                        .HasForeignKey("UserID1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("job_buddy_backend.Models.UserPhoneNumber", b =>
                {
                    b.HasOne("job_buddy_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("job_buddy_backend.Models.User", null)
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("UserID1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("JobBuddyBackend.Models.JobListing", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("JobTags");
                });

            modelBuilder.Entity("job_buddy_backend.Models.Resume", b =>
                {
                    b.Navigation("ATSScores");

                    b.Navigation("Applications");

                    b.Navigation("ResumeSkills");
                });

            modelBuilder.Entity("job_buddy_backend.Models.User", b =>
                {
                    b.Navigation("Educations");

                    b.Navigation("JobListings");

                    b.Navigation("PhoneNumbers");

                    b.Navigation("Resumes");
                });
#pragma warning restore 612, 618
        }
    }
}
