using AutoMapper;
using job_buddy_backend.Core.Interfaces.UserProfile;
using job_buddy_backend.DTO.UserProfile;
using job_buddy_backend.Models;
using job_buddy_backend.Models.DataContext;
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
                .ToListAsync();

            var userProfiles = users.Select(user => new UserProfileDto
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumbers.FirstOrDefault()?.PhoneNumber,
                Address = user.Address,
                ProfilePictureUrl = user.ProfilePictureUrl,
                LinkedInUrl = user.LinkedInUrl,
                DateOfBirth = user.DateOfBirth,
                Nationality = user.Nationality,
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
                ProfileCompletenessPercentage = CalculateProfileCompletenessAsync(user.UserID).Result
            }).ToList();

            return userProfiles;
        }

            public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
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
                LinkedInUrl = user.LinkedInUrl,
                DateOfBirth = user.DateOfBirth,
                Nationality = user.Nationality,
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
                ProfileCompletenessPercentage = await CalculateProfileCompletenessAsync(user.UserID)
            };
            

            return userProfile;
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserProfileDto updateDto)
        {
            var user = await _context.Users
                .Include(u => u.PhoneNumbers)
                .Include(u => u.Educations)
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) return false;

            _mapper.Map(updateDto, user);

            // Update phone numbers, educations, etc., based on provided data
            if (!string.IsNullOrWhiteSpace(updateDto.PhoneNumber))
            {
                var phoneNumber = user.PhoneNumbers.FirstOrDefault();
                if (phoneNumber == null)
                {
                    user.PhoneNumbers.Add(new UserPhoneNumber
                    {
                        PhoneNumber = updateDto.PhoneNumber,
                        UserID = user.UserID
                    });
                }
                else
                {
                    phoneNumber.PhoneNumber = updateDto.PhoneNumber;
                }
            }

            if (updateDto.Educations != null && updateDto.Educations.Any())
            {
                foreach (var educationDto in updateDto.Educations)
                {
                    var existingEducation = user.Educations.FirstOrDefault(e => e.UserEducationID == educationDto.UserEducationID);

                    if (existingEducation != null)
                    {
                        // Update the existing education entry
                        existingEducation.Degree = educationDto.Degree;
                        existingEducation.Institution = educationDto.Institution;
                        existingEducation.GraduationDate = educationDto.GraduationDate;
                    }
                    else
                    {
                        // Add new education entry
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

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId);
            if (user == null) throw new System.Exception("User not found.");

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
                .FirstOrDefaultAsync(u => u.UserID == userId);

            if (user == null) throw new System.Exception("User not found.");

            double completeness = 0;
            int totalFields = 9; // Number of fields considered for profile completeness

            completeness += !string.IsNullOrEmpty(user.FullName) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Email) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Address) ? 1 : 0;
            completeness += user.DateOfBirth.HasValue ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.Nationality) ? 1 : 0;
            completeness += user.PhoneNumbers.Any() ? 1 : 0;
            completeness += user.Educations.Any() ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.LinkedInUrl) ? 1 : 0;
            completeness += !string.IsNullOrEmpty(user.ProfilePictureUrl) ? 1 : 0;

            return (completeness / totalFields) * 100;
        }
    }
}
