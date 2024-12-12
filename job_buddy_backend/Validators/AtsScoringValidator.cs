using FluentValidation;
using job_buddy_backend.Controllers;

namespace job_buddy_backend.Validators
{
    public class AtsScoringValidator : AbstractValidator<AtsScoringRequest>
    {
        public AtsScoringValidator()
        {
            RuleFor(x => x.Resume).NotEmpty().WithMessage("Resume cannot be empty.");
            RuleFor(x => x.JobDescription).NotEmpty().WithMessage("Job Description cannot be empty.");
        }
    }
}
