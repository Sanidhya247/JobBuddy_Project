using FluentValidation;
using job_buddy_backend.DTO.UserProfile;

namespace job_buddy_backend.Validators
{
    public class UserProfileValidator : AbstractValidator<UserProfileDto>
    {
        public UserProfileValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.PhoneNumber).Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
                                        .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
        }
    }

    public class UserEducationValidator : AbstractValidator<UserEducationDto>
    {
        public UserEducationValidator()
        {
            RuleFor(e => e.Degree).NotEmpty().WithMessage("Degree is required.");
            RuleFor(e => e.Institution).NotEmpty().WithMessage("Institution name is required.");
        }
    }

    public class UserPhoneNumberValidator : AbstractValidator<UserPhoneNumberDto>
    {
        public UserPhoneNumberValidator()
        {
            RuleFor(p => p.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        }
    }

    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileDto>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        }
    }
}
