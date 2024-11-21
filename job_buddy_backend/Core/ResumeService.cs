using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using job_buddy_backend.Models.DataContext;
using job_buddy_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        
        var user = await _context.Users.FindAsync(resumeDto.UserID);
        if (user == null)
        {
            _logger.LogError($"User with ID {resumeDto.UserID} does not exist.");
            throw new ArgumentException("User does not exist.");
        }

       
        var existingResume = await _context.Resumes
            .FirstOrDefaultAsync(r => r.UserID == resumeDto.UserID && r.Title == resumeDto.Title);

        if (existingResume != null)
        {
            _logger.LogInformation($"A resume with title '{resumeDto.Title}' already exists for user {resumeDto.UserID}.");
            
            return new ResumeDto
            {
                ResumeID = existingResume.ResumeID,
                UserID = existingResume.UserID,
                Title = existingResume.Title,
                ResumeFileUrl = existingResume.ResumeFileUrl,
                ExperienceSummary = existingResume.ExperienceSummary,
                CreatedAt = existingResume.CreatedAt
            };
        }

        var resume = new Resume
        {
            UserID = resumeDto.UserID,
            Title = resumeDto.Title,
            ResumeFileUrl = resumeDto.ResumeFileUrl,
            ExperienceSummary = resumeDto.ExperienceSummary,
            CreatedAt = DateTime.UtcNow
        };

       
        await _context.Resumes.AddAsync(resume);
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
        
        var userExists = await _context.Users.AnyAsync(u => u.UserID == userId);
        if (!userExists)
        {
            _logger.LogError($"User with ID {userId} does not exist.");
            throw new ArgumentException("User does not exist.");
        }

        
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

    public async Task<bool> DeleteResumeAsync(int resumeId)
    {
        var resume = await _context.Resumes
            .Include(r => r.Applications)
            .Include(r => r.ATSScores)
            .Include(r => r.ResumeSkills)
            .FirstOrDefaultAsync(r => r.ResumeID == resumeId);

        if (resume == null)
        {
            _logger.LogError($"Resume with ID {resumeId} does not exist.");
            return false;
        }

     
        if (resume.Applications.Any())
        {
            _context.Applications.RemoveRange(resume.Applications);
        }

        if (resume.ATSScores.Any())
        {
            _context.ATSScores.RemoveRange(resume.ATSScores);
        }

        if (resume.ResumeSkills.Any())
        {
            _context.ResumeSkills.RemoveRange(resume.ResumeSkills);
        }

     
        _context.Resumes.Remove(resume);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Resume with ID {resumeId} and all related data successfully deleted.");
        return true;
    }

    public async Task<ResumeDto> UpdateResumeAsync(int resumeId, ResumeUploadDto resumeDto)
    {
        var resume = await _context.Resumes.FirstOrDefaultAsync(r => r.ResumeID == resumeId);
        if (resume == null)
        {
            _logger.LogError($"Resume with ID {resumeId} does not exist.");
            throw new ArgumentException("Resume does not exist.");
        }

       
        resume.Title = resumeDto.Title ?? resume.Title;
        resume.ResumeFileUrl = resumeDto.ResumeFileUrl ?? resume.ResumeFileUrl;
        resume.ExperienceSummary = resumeDto.ExperienceSummary ?? resume.ExperienceSummary;

        _context.Resumes.Update(resume);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Resume with ID {resumeId} successfully updated.");

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
}
