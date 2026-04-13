namespace Filmaholic.Shared.Records;

public record GetMovieRecord(
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