namespace Filmaholic.Shared.Records;

public record UpdateMovieRecord(
    string? Title,
    string? Genre,
    string? AgeGroup,
    int? Year,
    string? Description,
    string? UserName,
    byte[]? Image = null);