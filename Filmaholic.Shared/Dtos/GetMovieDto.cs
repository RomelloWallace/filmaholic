namespace Filmaholic.Shared.Dtos;

public record GetMovieDto(
    Guid Id,
    string Title,
    string Genre,
    int? Year,
    string? Description,
    string AgeGroup,
    string UserName,
    DateTime AddedAt,
    DateTime UpdatedAt,
    byte[]? Image);