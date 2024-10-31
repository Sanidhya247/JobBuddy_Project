using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models;
using Microsoft.EntityFrameworkCore;

public class ResumeService : IResumeService
{
    private readonly JobBuddyDbContext _context;
    private readonly ILogger<ResumeService> _logger;

    public ResumeService(JobBuddyDbContext context, ILogger<ResumeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResumeDto> UploadResumeAsync(ResumeUploadDto resumeDto)
    {
        var resume = new Resume
        {
            UserID = resumeDto.UserID,
            Title = resumeDto.Title,
            ResumeFileUrl = resumeDto.ResumeFileUrl,  // Store the URL of the uploaded file
            ExperienceSummary = resumeDto.ExperienceSummary,
            CreatedAt = DateTime.UtcNow
        };

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();

        return new ResumeDto
        {
            ResumeID = resume.ResumeID,
            UserID = resume.UserID,
            Title = resume.Title,
            ResumeFileUrl = resume.ResumeFileUrl,
            ExperienceSummary = resume.ExperienceSummary,
            CreatedAt = resume.CreatedAt
        };
    }

    public async Task<IEnumerable<ResumeDto>> GetResumesByUserIdAsync(int userId)
    {
        var resumes = await _context.Resumes
            .Where(r => r.UserID == userId)
            .ToListAsync();

        return resumes.Select(resume => new ResumeDto
        {
            ResumeID = resume.ResumeID,
            UserID = resume.UserID,
            Title = resume.Title,
            ResumeFileUrl = resume.ResumeFileUrl,
            ExperienceSummary = resume.ExperienceSummary,
            CreatedAt = resume.CreatedAt
        });
    }

    public Task<bool> DeleteResumeAsync(int resumeId)
    {
        throw new NotImplementedException();
    }
}
