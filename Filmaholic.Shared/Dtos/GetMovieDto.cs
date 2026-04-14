namespace Filmaholic.Shared.Dtos;

public class GetMovieDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int? Year { get; set; }

    public string? Description { get; set; }

    public string AgeGroup { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public DateTime AddedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[]? Image { get; set; }
}