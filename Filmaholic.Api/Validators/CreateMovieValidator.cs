using Filmaholic.Api.Requests;
using FluentValidation;

namespace Filmaholic.Api.Validators;

public sealed class CreateMovieValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Genre is required.");

        RuleFor(x => x.AgeGroup)
            .NotEmpty()
            .WithMessage("Age group is required.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("User name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Year)
            .InclusiveBetween(1888, DateTime.UtcNow.Year)
            .WithMessage("Year must be a valid film year.");

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
            .WithMessage("Image must be less than 5MB.");
    }
}