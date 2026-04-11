namespace Filmaholic.Api.Records;

public record GetMoviesRecord(
    Guid Id,
    string Title,
    string Genre,
    int? Year,
    string? Description,
    string AgeGroup,
    string UserName,
    DateTime AddedAt,
    DateTime UpdatedAt);