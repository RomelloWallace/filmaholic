namespace Filmaholic.Api.Records;

public record CreateMovieRecord(
    string Title,
    string Genre,
    string AgeGroup,
    string UserName,
    int? Year,
    string? Description,
    byte[]? Image = null);