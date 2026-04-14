namespace Filmaholic.Shared.Dtos;

public class UpdateMovieDto
{
    public string? Title { get; set; }

    public string? Genre { get; set; }

    public string? AgeGroup { get; set; }

    public int? Year { get; set; }

    public string? Description { get; set; }

    public string? UserName { get; set; }

    public byte[]? Image { get; set; }
}