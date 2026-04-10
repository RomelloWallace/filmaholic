namespace Filmaholic.Api.Dtos
{
    public record UpdateMovieDto(
        Guid Id,
        string Title,
        string? Genre,
        int Year,
        string? Description,
        DateTime AddedAt,
        DateTime UpdatedAt,
        string UserName,
        byte[]? Image);
}