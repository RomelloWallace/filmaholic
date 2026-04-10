namespace Filmaholic.Api.Dtos
{
    public record CreateMovieDto(
        string Title,
        string Genre,
        string AgeGroup,
        int? Year,
        string? Description,
        string UserName,
        byte[]? Image);
}