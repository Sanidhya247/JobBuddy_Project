using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Services
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly List<EmployerProfileDto> _profiles = new();

        public Task<EmployerProfileDto> GetProfileByIdAsync(int profileId)
        {
            var profile = _profiles.FirstOrDefault(p => p.EmployerProfileID == profileId);
            return Task.FromResult(profile);
        }

        public Task<IEnumerable<EmployerProfileDto>> GetAllProfilesAsync()
        {
            return Task.FromResult(_profiles.AsEnumerable());
        }

        public Task<EmployerProfileDto> CreateProfileAsync(EmployerProfileDto employerProfileDto)
        {
            employerProfileDto.EmployerProfileID = _profiles.Count + 1; // Simple auto-increment ID for demo
            _profiles.Add(employerProfileDto);
            return Task.FromResult(employerProfileDto);
        }

        public Task<EmployerProfileDto> UpdateProfileAsync(int profileId, EmployerProfileDto employerProfileDto)
        {
            var profile = _profiles.FirstOrDefault(p => p.EmployerProfileID == profileId);
            if (profile != null)
            {
                profile.CompanyName = employerProfileDto.CompanyName;
                profile.ContactPerson = employerProfileDto.ContactPerson;
                profile.ContactEmail = employerProfileDto.ContactEmail;
                profile.PhoneNumber = employerProfileDto.PhoneNumber;
            }
            return Task.FromResult(profile);
        }

        public Task<bool> DeleteProfileAsync(int profileId)
        {
            var profile = _profiles.FirstOrDefault(p => p.EmployerProfileID == profileId);
            if (profile != null)
            {
                _profiles.Remove(profile);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
