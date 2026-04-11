namespace Filmaholic.Api.Forms;

public class UpdateMovieForm
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? AgeGroup { get; set; }
    public int? Year { get; set; }
    public string? Description { get; set; }
    public string? UserName { get; set; }
    public IFormFile? Image { get; set; }
}