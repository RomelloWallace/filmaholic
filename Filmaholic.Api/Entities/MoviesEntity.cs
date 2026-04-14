namespace Filmaholic.Api.Entities
{
public class MovieEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string AgeGroup { get; set; } = default!;
    public string UserName { get; set; } = default!;

    public int? Year { get; set; }
    public string? Description { get; set; }

    public byte[]? Image { get; set; }

    public DateTime AddedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
}