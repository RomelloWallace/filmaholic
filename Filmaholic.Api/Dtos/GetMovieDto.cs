namespace Filmaholic.Api.Dtos
{
    public record GetMovieDto(
        Guid Id,
        string Title,
        string? Genre,
        string AgeGroup,
        int? Year,
        string? Description,
        DateTime AddedAt,
        DateTime UpdatedAt,
        string UserName,
        byte[]? Image);
}