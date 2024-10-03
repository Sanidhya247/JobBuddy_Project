using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.Core;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using Microsoft.EntityFrameworkCore;


namespace job_buddy_backend.Core
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly JobBuddyDbContext _context;

        public EmployerProfileService(JobBuddyDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerProfileDto> GetProfileByIdAsync(int profileId)
        {
            var profile = await _context.EmployerProfiles.FindAsync(profileId);
            if (profile == null) return null;

            return new EmployerProfileDto
            {
                EmployerProfileID = profile.EmployerProfileID,
                CompanyName = profile.CompanyName,
                CompanyWebsite = profile.CompanyWebsite,
                ContactPerson = profile.ContactPerson,
                ContactEmail = profile.ContactEmail,
                PhoneNumber = profile.PhoneNumber,
                OfficeAddress = profile.OfficeAddress,
                City = profile.City,
                Province = profile.Province,
                ZipCode = profile.ZipCode
            };
        }

        public async Task<IEnumerable<EmployerProfileDto>> GetAllProfilesAsync()
        {
            return await _context.EmployerProfiles
                .Select(profile => new EmployerProfileDto
                {
                    EmployerProfileID = profile.EmployerProfileID,
                    CompanyName = profile.CompanyName,
                    CompanyWebsite = profile.CompanyWebsite,
                    ContactPerson = profile.ContactPerson,
                    ContactEmail = profile.ContactEmail,
                    PhoneNumber = profile.PhoneNumber,
                    OfficeAddress = profile.OfficeAddress,
                    City = profile.City,
                    Province = profile.Province,
                    ZipCode = profile.ZipCode
                }).ToListAsync();
        }

        public async Task<EmployerProfileDto> CreateProfileAsync(EmployerProfileDto employerProfileDto)
        {
            var profile = new EmployerProfile
            {
                CompanyName = employerProfileDto.CompanyName,
                CompanyWebsite = employerProfileDto.CompanyWebsite,
                ContactPerson = employerProfileDto.ContactPerson,
                ContactEmail = employerProfileDto.ContactEmail,
                PhoneNumber = employerProfileDto.PhoneNumber,
                OfficeAddress = employerProfileDto.OfficeAddress,
                City = employerProfileDto.City,
                Province = employerProfileDto.Province,
                ZipCode = employerProfileDto.ZipCode
            };

            _context.EmployerProfiles.Add(profile);
            await _context.SaveChangesAsync();

            return employerProfileDto;
        }

        public async Task<EmployerProfileDto> UpdateProfileAsync(int profileId, EmployerProfileDto employerProfileDto)
        {
            var profile = await _context.EmployerProfiles.FindAsync(profileId);
            if (profile == null) return null;

            profile.CompanyName = employerProfileDto.CompanyName;
            profile.CompanyWebsite = employerProfileDto.CompanyWebsite;
            profile.ContactPerson = employerProfileDto.ContactPerson;
            profile.ContactEmail = employerProfileDto.ContactEmail;
            profile.PhoneNumber = employerProfileDto.PhoneNumber;
            profile.OfficeAddress = employerProfileDto.OfficeAddress;
            profile.City = employerProfileDto.City;
            profile.Province = employerProfileDto.Province;
            profile.ZipCode = employerProfileDto.ZipCode;

            await _context.SaveChangesAsync();
            return employerProfileDto;
        }

        public async Task<bool> DeleteProfileAsync(int profileId)
        {
            var profile = await _context.EmployerProfiles.FindAsync(profileId);
            if (profile == null) return false;

            _context.EmployerProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return true;
        }

        Task<EmployerProfileDto> IEmployerProfileService.GetProfileByIdAsync(int profileId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<EmployerProfileDto>> IEmployerProfileService.GetAllProfilesAsync()
        {
            throw new NotImplementedException();
        }

        //public Task<EmployerProfileDto> CreateProfileAsync(EmployerProfileDto employerProfileDto)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<EmployerProfileDto> UpdateProfileAsync(int profileId, EmployerProfileDto employerProfileDto)
        //{
        //    throw new NotImplementedException();
        //}
    }
}