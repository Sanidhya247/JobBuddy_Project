using FluentValidation;
using JobBuddyBackend.Models;

namespace job_buddy_backend.Validators
{
    public class ATSScoreValidator : AbstractValidator<ATSScore>
    {
        public ATSScoreValidator()
        {
            RuleFor(x => x.ResumeID)
                .GreaterThan(0).WithMessage("Resume ID must be a positive number.");

            RuleFor(x => x.JobID)
                .GreaterThan(0).WithMessage("Job ID must be a positive number.");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100.");
        }
    }
}
