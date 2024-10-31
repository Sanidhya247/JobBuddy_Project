using job_buddy_backend.DTO;
using job_buddy_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace job_buddy_backend.Core.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeDto> UploadResumeAsync(ResumeUploadDto resumeDto);
        Task<IEnumerable<ResumeDto>> GetResumesByUserIdAsync(int userId);
        Task<bool> DeleteResumeAsync(int resumeId);
    }
}
