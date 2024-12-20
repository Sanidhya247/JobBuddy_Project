﻿using FluentValidation;
using job_buddy_backend.Models;
using JobBuddyBackend.Models;

namespace job_buddy_backend.Validators
{
    public class ResumeValidator : AbstractValidator<Resume>
    {
        public ResumeValidator()
        {
            RuleFor(x => x.UserID)
                .GreaterThan(0).WithMessage("User ID must be a positive number.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

        }
    }
}
