namespace Filmaholic.Api.Dtos
{
    public record UpdateMovieDto(
        string Title,
        string Genre,
        int? Year,
        string? Description,
        string AgeGroup,
        byte[]? Image);
}
