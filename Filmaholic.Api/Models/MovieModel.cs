namespace Filmaholic.Api.Models
{
    public class MovieModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int Year { get; set; }
        public string? Description { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
    }
}