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

            RuleFor(x => x.CoverLetter)
                .MaximumLength(1000).WithMessage("Cover letter cannot exceed 1000 characters.");

            // FirstName validation
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            // LastName validation
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // Phone validation (optional)
            RuleFor(x => x.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be valid.");

            // Date of Birth validation (must be in the past)
            RuleFor(x => x.Dob)
                .LessThan(DateTime.Today).WithMessage("Date of Birth must be in the past.");


            // Linkedin validation (optional but must be a valid URL if provided)
            RuleFor(x => x.Linkedin)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .When(x => !string.IsNullOrEmpty(x.Linkedin))
                .WithMessage("Linkedin URL must be valid.");
        }
    }   
}
