using FluentValidation;
using job_buddy_backend.DTO;

namespace job_buddy_backend.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {

        public ForgotPasswordValidator()
        {
            // Email is required and must be a valid email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

        }

    }
}
