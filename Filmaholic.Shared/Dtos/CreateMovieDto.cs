namespace Filmaholic.Shared.Dtos;

public record CreateMovieDto(
    string Title,
    string Genre,
    string AgeGroup,
    string UserName,
    int? Year,
    string? Description,
    byte[]? Image = null);