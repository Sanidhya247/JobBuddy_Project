using FluentValidation;
using job_buddy_backend.DTO.UserProfile;

namespace job_buddy_backend.Validators
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required.")
                .When(x => x.Nationality != null);
        }
    }
}
