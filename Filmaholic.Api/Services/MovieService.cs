using Filmaholic.Api.Dtos;
using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Models;
using Microsoft.AspNetCore.Builder;

namespace Filmaholic.Api.Services;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        app.MapGet("/movies", async (IMovie service) =>
        {
            var movies = await service.GetAllMovies();
            return Results.Ok(movies);
        });

        app.MapPost("/movies", async (CreateMovieDto newMovie, IMovie service) =>
        {
            var movie = await service.AddMovie(newMovie);
            return Results.Created($"/movies/{movie.Id}", movie);
        });
    }
}
