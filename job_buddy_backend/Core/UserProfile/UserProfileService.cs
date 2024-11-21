using AutoMapper;
using job_buddy_backend.Core.Interfaces.UserProfile;
using job_buddy_backend.DTO.UserProfile;
using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models.UserModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        private readonly JobBuddyDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(JobBuddyDbContext context, IMapper mapper, ILogger<UserProfileService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserProfileDto>> GetAllUserProfilesAsync()
        {
            var users = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Certifications)
                .Include(u => u.Projects)
                .ToListAsync();

            var userProfiles = users.Select(user => new UserProfileDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumbers.FirstOrDefault()?.PhoneNumber,
                Address = user.Address,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CoverPhotoUrl = user.CoverPhotoUrl,
                LinkedInUrl = user.LinkedInUrl,
                Headline = user.Headline,
                About = user.About,
                DateOfBirth = user.DateOfBirth,
                Nationality = user.Nationality,
                IsPremium = user.IsPremium,
                IsActive = user.IsActive,
                Educations = user.Educations.Select(e => new UserEducationDto
                {
                    UserEducationID = e.UserEducationID,
                    Degree = e.Degree,
                    Institution = e.Institution,
                    GraduationDate = e.GraduationDate
                }).ToList(),
                PhoneNumbers = user.PhoneNumbers.Select(p => new UserPhoneNumberDto
                {
                    UserPhoneNumberID = p.UserPhoneNumberID,
                    PhoneNumber = p.PhoneNumber,
                    Type = p.Type
                }).ToList(),
                Experiences = user.Experiences.Select(exp => new UserExperienceDto
                {
                    UserExperienceID = exp.UserExperienceID,
                    JobTitle = exp.JobTitle,
                    Company = exp.Company,
                    Location = exp.Location,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    Description = exp.Description
                }).ToList(),
                Certifications = user.Certifications.Select(cert => new UserCertificationDto
                {
                    UserCertificationID = cert.UserCertificationID,
                    Title = cert.Title,
                    IssuedBy = cert.IssuedBy,
                    IssueDate = cert.IssueDate,
                    CredentialUrl = cert.CredentialUrl
                }).ToList(),
                Projects = user.Projects.Select(proj => new UserProjectDto
                {
                    UserProjectID = proj.UserProjectID,
                    ProjectTitle = proj.ProjectTitle,
                    Description = proj.Description,
                    StartDate = proj.StartDate,
                    EndDate = proj.EndDate,
                    Link = proj.Link
                }).ToList(),
                ProfileCompletenessPercentage = CalculateProfileCompletenessAsync(user.UserID).Result
            }).ToList();

            return userProfiles;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Certifications)
                .Include(u => u.Projects)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null)
                throw new System.Exception("User not found");

            var userProfile = new UserProfileDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumbers.FirstOrDefault()?.PhoneNumber,
                Address = user.Address,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CoverPhotoUrl = user.CoverPhotoUrl,
                LinkedInUrl = user.LinkedInUrl,
                Headline = user.Headline,
                About = user.About,
                DateOfBirth = user.DateOfBirth,
                Nationality = user.Nationality,
                IsPremium = user.IsPremium,
                IsActive = user.IsActive,
                Educations = user.Educations.Select(e => new UserEducationDto
                {
                    UserEducationID = e.UserEducationID,
                    Degree = e.Degree,
                    Institution = e.Institution,
                    GraduationDate = e.GraduationDate
                }).ToList(),
                PhoneNumbers = user.PhoneNumbers.Select(p => new UserPhoneNumberDto
                {
                    UserPhoneNumberID = p.UserPhoneNumberID,
                    PhoneNumber = p.PhoneNumber,
                    Type = p.Type
                }).ToList(),
                Experiences = user.Experiences.Select(exp => new UserExperienceDto
                {
                    UserExperienceID = exp.UserExperienceID,
                    JobTitle = exp.JobTitle,
                    Company = exp.Company,
                    Location = exp.Location,
                    StartDate = exp.StartDate,
                    EndDate = exp.EndDate,
                    Description = exp.Description
                }).ToList(),
                Certifications = user.Certifications.Select(cert => new UserCertificationDto
                {
                    UserCertificationID = cert.UserCertificationID,
                    Title = cert.Title,
                    IssuedBy = cert.IssuedBy,
                    IssueDate = cert.IssueDate,
                    CredentialUrl = cert.CredentialUrl
                }).ToList(),
                Projects = user.Projects.Select(proj => new UserProjectDto
                {
                    UserProjectID = proj.UserProjectID,
                    ProjectTitle = proj.ProjectTitle,
                    Description = proj.Description,
                    StartDate = proj.StartDate,
                    EndDate = proj.EndDate,
                    Link = proj.Link
                }).ToList(),
                ProfileCompletenessPercentage = await CalculateProfileCompletenessAsync(user.UserID)
            };

            return userProfile;
        }


        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto)
        {
            var user = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Certifications)
                .Include(u => u.Projects)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return false;

            // Update main fields
            user.FullName = updateDto.FullName ?? user.FullName;
            user.About = updateDto.About ?? user.About;
            user.Headline = updateDto.Headline ?? user.Headline;
            user.Address = updateDto.Address ?? user.Address;
            user.DateOfBirth = updateDto.DateOfBirth ?? user.DateOfBirth;
            user.Nationality = updateDto.Nationality ?? user.Nationality;
            user.LinkedInUrl = updateDto.LinkedInUrl ?? user.LinkedInUrl;
            

            // Update Phone Numbers
            if (updateDto.PhoneNumbers != null && updateDto.PhoneNumbers.Any())
            {
                foreach (var phoneDto in updateDto.PhoneNumbers)
                {
                    var existingPhone = user.PhoneNumbers.FirstOrDefault(p => p.UserPhoneNumberID == phoneDto.UserPhoneNumberID);
                    if (existingPhone != null)
                    {
                        existingPhone.PhoneNumber = phoneDto.PhoneNumber;
                        existingPhone.Type = phoneDto.Type;
                    }
                    else
                    {
                        user.PhoneNumbers.Add(new UserPhoneNumber
                        {
                            PhoneNumber = phoneDto.PhoneNumber,
                            Type = phoneDto.Type,
                            UserID = user.UserID
                        });
                    }
                }
            }

            // Update Educations
            if (updateDto.Educations != null && updateDto.Educations.Any())
            {
                foreach (var educationDto in updateDto.Educations)
                {
                    var existingEducation = user.Educations.FirstOrDefault(e => e.UserEducationID == educationDto.UserEducationID);

                    if (existingEducation != null)
                    {
                        existingEducation.Degree = educationDto.Degree;
                        existingEducation.Institution = educationDto.Institution;
                        existingEducation.GraduationDate = educationDto.GraduationDate;
                    }
                    else
                    {
                        user.Educations.Add(new UserEducation
                        {
                            Degree = educationDto.Degree,
                            Institution = educationDto.Institution,
                            GraduationDate = educationDto.GraduationDate,
                            UserID = user.UserID
                        });
                    }
                }
            }

            // Update Experiences
            if (updateDto.Experiences != null && updateDto.Experiences.Any())
            {
                foreach (var experienceDto in updateDto.Experiences)
                {
                    var existingExperience = user.Experiences.FirstOrDefault(e => e.UserExperienceID == experienceDto.UserExperienceID);

                    if (existingExperience != null)
                    {
                        existingExperience.JobTitle = experienceDto.JobTitle;
                        existingExperience.Company = experienceDto.Company;
                        existingExperience.Location = experienceDto.Location;
                        existingExperience.StartDate = experienceDto.StartDate;
                        existingExperience.EndDate = experienceDto.EndDate;
                        existingExperience.Description = experienceDto.Description;
                    }
                    else
                    {
                        user.Experiences.Add(new UserExperience
                        {
                            JobTitle = experienceDto.JobTitle,
                            Company = experienceDto.Company,
                            Location = experienceDto.Location,
                            StartDate = experienceDto.StartDate,
                            EndDate = experienceDto.EndDate,
                            Description = experienceDto.Description,
                            UserID = user.UserID
                        });
                    }
                }
            }

            // Update Certifications
            if (updateDto.Certifications != null && updateDto.Certifications.Any())
            {
                foreach (var certificationDto in updateDto.Certifications)
                {
                    var existingCertification = user.Certifications.FirstOrDefault(c => c.UserCertificationID == certificationDto.UserCertificationID);

                    if (existingCertification != null)
                    {
                        existingCertification.Title = certificationDto.Title;
                        existingCertification.IssuedBy = certificationDto.IssuedBy;
                        existingCertification.IssueDate = certificationDto.IssueDate;
                        existingCertification.CredentialUrl = certificationDto.CredentialUrl;
                    }
                    else
                    {
                        user.Certifications.Add(new UserCertification
                        {
                            Title = certificationDto.Title,
                            IssuedBy = certificationDto.IssuedBy,
                            IssueDate = certificationDto.IssueDate,
                            CredentialUrl = certificationDto.CredentialUrl,
                            UserID = user.UserID
                        });
                    }
                }
            }

            // Update Projects
            if (updateDto.Projects != null && updateDto.Projects.Any())
            {
                foreach (var projectDto in updateDto.Projects)
                {
                    var existingProject = user.Projects.FirstOrDefault(p => p.UserProjectID == projectDto.UserProjectID);

                    if (existingProject != null)
                    {
                        existingProject.ProjectTitle = projectDto.ProjectTitle;
                        existingProject.Description = projectDto.Description;
                        existingProject.StartDate = projectDto.StartDate;
                        existingProject.EndDate = projectDto.EndDate;
                        existingProject.Link = projectDto.Link;
                    }
                    else
                    {
                        user.Projects.Add(new UserProject
                        {
                            ProjectTitle = projectDto.ProjectTitle,
                            Description = projectDto.Description,
                            StartDate = projectDto.StartDate,
                            EndDate = projectDto.EndDate,
                            Link = projectDto.Link,
                            UserID = user.UserID
                        });
                    }
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<string> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null) throw new System.Exception("User not found.");

            
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                var oldFilePath = Path.Combine("wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            var fileName = $"{userId}_{System.Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine("wwwroot", "profile-pictures", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.ProfilePictureUrl = $"/profile-pictures/{fileName}";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.ProfilePictureUrl;
        }

        public async Task<string> UploadCoverPhotoAsync(int userId, IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null) throw new System.Exception("User not found.");

            // Delete existing cover photo
            if (!string.IsNullOrEmpty(user.CoverPhotoUrl))
            {
                var oldFilePath = Path.Combine("wwwroot", user.CoverPhotoUrl.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            var fileName = $"{userId}_cover_{System.Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine("wwwroot", "cover-photos", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            user.CoverPhotoUrl = $"/cover-photos/{fileName}";
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.CoverPhotoUrl;
        }

        public async Task<bool> RemoveProfilePictureAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null || string.IsNullOrEmpty(user.ProfilePictureUrl))
                throw new System.Exception("Profile picture not found.");

            var filePath = Path.Combine("wwwroot", user.ProfilePictureUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            user.ProfilePictureUrl = null;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<double> CalculateProfileCompletenessAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
                .Include(u => u.Experiences)
                .Include(u => u.Certifications)
                .Include(u => u.Projects)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) throw new System.Exception("User not found.");

            double completeness = 0;
            int totalFields = 14; // Number of fields considered for profile completeness

            completeness += !string.IsNullOrEmpty(user.FullName) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Email) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Address) ? 1 : 0;
            completeness += user.DateOfBirth.HasValue ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Nationality) ? 1 : 0;
            completeness += user.PhoneNumbers.Any() ? 1 : 0;
            completeness += user.Educations.Any() ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.LinkedInUrl) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.ProfilePictureUrl) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.CoverPhotoUrl) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Headline) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.About) ? 1 : 0;
            completeness += user.Experiences.Any() ? 1 : 0;
            completeness += user.Certifications.Any() ? 1 : 0;

            return (completeness / totalFields) * 100;
        }

    }
}
