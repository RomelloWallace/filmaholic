namespace Filmaholic.Api.Dtos
{
    public record CreateMovieDto(
        string Title,
        string? Genre,
        int Year,
        string? Description,
        string UserName);
}