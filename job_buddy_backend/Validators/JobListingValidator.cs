using FluentValidation;
using JobBuddyBackend.Models;

namespace job_buddy_backend.Validators
{
    public class JobListingValidator : AbstractValidator<JobListing>
    {
        public JobListingValidator() 
        {
            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("Job title is required.")
                .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters.");

            RuleFor(x => x.JobDescription)
                .NotEmpty().WithMessage("Job description is required.");
            //commenting out  for the updated dependent model from other team member
            //RuleFor(x => x.City)
            //    .NotEmpty().WithMessage("City is required.");

            //RuleFor(x => x.Province)
            //    .NotEmpty().WithMessage("Province is required.");

            //RuleFor(x => x.ZipCode)
            //    .NotEmpty().WithMessage("ZipCode is required.");

            //RuleFor(x => x.WorkType)
            //    .NotEmpty().WithMessage("WorkType is required.")
            //    .Must(x => new[] { "Remote", "Hybrid", "In-Person" }.Contains(x))
            //    .WithMessage("WorkType must be 'Remote', 'Hybrid', or 'In-Person'.");

            //RuleFor(x => x.PayRatePerHour)
            //    .GreaterThanOrEqualTo(0).WithMessage("Pay rate per hour cannot be negative.")
            //    .When(x => x.PayRatePerHour.HasValue);

            //RuleFor(x => x.PayRatePerYear)
            //    .GreaterThanOrEqualTo(0).WithMessage("Pay rate per year cannot be negative.")
            //    .When(x => x.PayRatePerYear.HasValue);
        }
    }
}
