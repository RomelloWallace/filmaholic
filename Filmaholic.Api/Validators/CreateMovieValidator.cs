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
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .Must(genre => !string.IsNullOrWhiteSpace(genre))
            .WithMessage("Genre is required.");

        RuleFor(x => x.AgeGroup)
            .NotEmpty()
            .Must(ageGroup => !string.IsNullOrWhiteSpace(ageGroup))
            .WithMessage("Age group is required.");

        RuleFor(x => x.Year)
            .InclusiveBetween(1888, DateTime.UtcNow.Year)
            .WithMessage("Year must be a valid film year.")
            .When(x => x.Year.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.Image)
            .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
            .WithMessage("Image must be 10MB or smaller.");
    }
}