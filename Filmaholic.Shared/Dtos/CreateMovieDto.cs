namespace Filmaholic.Shared.Dtos;

public class CreateMovieDto
{
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int? Year { get; set; }
    public string? Description { get; set; }
    public byte[]? Image { get; set; }
}