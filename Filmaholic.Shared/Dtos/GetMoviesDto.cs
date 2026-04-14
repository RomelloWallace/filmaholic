namespace Filmaholic.Shared.Dtos;

public record GetMoviesDto(
    Guid Id,
    string Title,
    string Genre,
    int? Year,
    string? Description,
    string AgeGroup,
    string UserName,
    DateTime AddedAt,
    DateTime UpdatedAt);