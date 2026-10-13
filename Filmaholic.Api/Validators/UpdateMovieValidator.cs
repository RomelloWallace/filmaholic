using Filmaholic.Api.Requests;
using FluentValidation;

namespace Filmaholic.Api.Validators;

public sealed class UpdateMovieValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title must not be empty.")
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title must not be empty.")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.")
            .When(x => x.Title is not null);

        RuleFor(x => x.Genre)
            .NotEmpty()
            .Must(genre => !string.IsNullOrWhiteSpace(genre))
            .WithMessage("Genre must not be empty.")
            .When(x => x.Genre is not null);

        RuleFor(x => x.AgeGroup)
            .NotEmpty()
            .Must(ageGroup => !string.IsNullOrWhiteSpace(ageGroup))
            .WithMessage("Age group must not be empty.")
            .When(x => x.AgeGroup is not null);

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
