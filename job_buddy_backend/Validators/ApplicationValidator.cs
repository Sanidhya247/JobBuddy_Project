using FluentValidation;
using job_buddy_backend.Models;

namespace job_buddy_backend.Validators
{
    public class ApplicationValidator : AbstractValidator<Application>
    {
        public ApplicationValidator()
        {
            RuleFor(x => x.JobID)
                .GreaterThan(0).WithMessage("Job ID must be a positive number.");

            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID must be a positive number.");

            RuleFor(x => x.ResumeID)
                .GreaterThan(0).WithMessage("Resume ID must be a positive number.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status cannot be empty.");

            RuleFor(x => x.CoverLetter)
                .MaximumLength(500).WithMessage("Cover letter cannot exceed 500 characters.");
        }
    }
}
