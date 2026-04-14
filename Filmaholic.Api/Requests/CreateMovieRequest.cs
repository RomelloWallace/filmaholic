namespace Filmaholic.Api.Requests;
public class CreateMovieRequest
{
    public string Title { get; set; } = default!;
    public string Genre { get; set; } = default!;
    public string AgeGroup { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public int? Year { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
}