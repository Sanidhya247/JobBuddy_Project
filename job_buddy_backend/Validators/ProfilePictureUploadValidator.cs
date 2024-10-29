using FluentValidation;
using job_buddy_backend.DTO.UserProfile;

namespace job_buddy_backend.Validators
{
    public class ProfilePictureUploadValidator : AbstractValidator<ProfilePictureUploadDto>
    {
        public ProfilePictureUploadValidator()
        {
            RuleFor(x => x.ProfilePicture.Length).LessThanOrEqualTo(5242880)
                .WithMessage("File size must be less than 5MB.");

            RuleFor(x => x.ProfilePicture.ContentType).Must(type => type.Equals("image/jpeg") || type.Equals("image/png"))
                .WithMessage("Only JPEG or PNG images are allowed.");
        }
    }
}
