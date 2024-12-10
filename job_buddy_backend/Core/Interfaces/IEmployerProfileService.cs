using job_buddy_backend.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IEmployerProfileService
    {
        Task<EmployerProfileDto> GetProfileByIdAsync(int profileId);
        Task<IEnumerable<EmployerProfileDto>> GetAllProfilesAsync();
        Task<EmployerProfileDto> CreateProfileAsync(EmployerProfileDto employerProfileDto);
        Task<EmployerProfileDto> UpdateProfileAsync(int profileId, EmployerProfileDto employerProfileDto);
        Task<bool> DeleteProfileAsync(int profileId);
    }
}
