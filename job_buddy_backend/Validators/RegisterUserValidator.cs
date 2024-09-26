using FluentValidation;
using job_buddy_backend.DTO;

namespace job_buddy_backend.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator()
        {
            // Full Name is required and should not be empty
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .Length(2, 100).WithMessage("Full name must be between 2 and 100 characters.");

            // Email is required and must be in a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            // Password validation: At least 6 characters, 1 uppercase, 1 number, 1 special character
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");


            // Role should be either "Job Seeker" or "Employer"
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Job Seeker" || role == "Employer")
                .WithMessage("Role must be either 'Job Seeker' or 'Employer'.");

            // Phone Number is optional, but if provided, it should be valid
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is invalid.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // LinkedIn URL is optional, but if provided, it should be a valid URL
            RuleFor(x => x.LinkedInUrl)
                .Must(IsValidUrl).WithMessage("A valid LinkedIn URL is required.")
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl));

            // Address is optional but should not exceed 255 characters if provided
            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address cannot be longer than 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }

        // Helper function to validate URLs
        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
