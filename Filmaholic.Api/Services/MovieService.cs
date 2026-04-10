using Filmaholic.Api.Dtos;
using Filmaholic.Api.Interfaces;

namespace Filmaholic.Api.Services;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        app.MapGet("filmaholic/v1/movies", async (IMovie service) =>
        {
            var movies = await service.GetAllMovies();
            return Results.Ok(movies);
        });

        app.MapPost("filmaholic/v1/movies", async (HttpRequest request, IMovie service) =>
        {
            var form = await request.ReadFormAsync();
            
            var title = form["Title"].ToString();
            var genre = form["Genre"].ToString();
            var ageGroup = form["AgeGroup"].ToString();
            var year = int.TryParse(form["Year"], out var y) ? y : (int?)null;
            var description = form["Description"].ToString();
            var userName = form["UserName"].ToString();
            
            var file = form.Files.GetFile("Image");

            byte[]? imageBytes = null;

            if (file is not null)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var dto = new CreateMovieDto(
                title,
                genre,
                ageGroup,
                year,
                description,
                userName,
                imageBytes
            );

            var movie = await service.AddMovie(dto);
            return Results.Created($"filmaholic/v1/movies/{movie.Id}", movie);
        })
        .Accepts<IFormFile>("multipart/form-data");
        
        app.MapGet("filmaholic/v1/movies/{movieId}", async (Guid movieId, IMovie service) =>
        {
            var movie = await service.GetMovieById(movieId);
            return Results.Ok(movie);
        });
        
        app.MapDelete("filmaholic/v1/movies/{movieId}", async (Guid movieId, IMovie service) =>
        {
            var movieDeleted = await service.DeleteMovie(movieId);
            return Results.Ok(movieDeleted);
        });
        
        app.MapPatch("filmaholic/v1/movies/{movieId}", async (Guid movieId, UpdateMovieDto updateMovie, IMovie service) =>
        {
            var movieUpdated = await service.UpdateMovie(movieId, updateMovie);
            return Results.Ok(movieUpdated);
        });
    }
}
