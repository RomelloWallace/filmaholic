namespace Filmaholic.Shared.Dtos;

public record UpdateMovieDto(
    string? Title,
    string? Genre,
    string? AgeGroup,
    int? Year,
    string? Description,
    string? UserName,
    byte[]? Image = null);