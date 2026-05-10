using Filmaholic.Shared.Dtos;
using Filmaholic.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Filmaholic.Api.Requests;

namespace Filmaholic.Api.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("filmaholic/v1/movies");

        // GET ALL
        group.MapGet("/", async (IMovieService service, CancellationToken ct) =>
        {
            var movies = await service.GetAllMovies(ct);
            return Results.Ok(movies);
        });

        // GET BY ID
        group.MapGet("/{movieId:guid}", async (Guid movieId, IMovieService service) =>
        {
            var movie = await service.GetMovieById(movieId);
            return Results.Ok(movie);
        });

        // CREATE
        group.MapPost("/", async (
            [FromForm] CreateMovieRequest form,
            IMovieService service,
            CancellationToken ct) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var request = new CreateMovieDto
            {
                Title = form.Title,
                Genre = form.Genre,
                AgeGroup = form.AgeGroup,
                UserName = form.UserName,
                Year = form.Year,
                Description = form.Description,
                Image = imageBytes
            };
                        var movie = await service.AddMovie(request, ct);

            return TypedResults.Created(
                $"/filmaholic/v1/movies/{movie.Id}",
                movie);
        });

        // UPDATE (PATCH)
        group.MapPatch("/{movieId:guid}/edit", async (
            Guid movieId,
            [FromForm] UpdateMovieRequest form,
            IMovieService service,
            CancellationToken ct) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms, ct);
                imageBytes = ms.ToArray();
            }

            var dto = new UpdateMovieDto
            {
                Title = form.Title,
                Genre = form.Genre,
                AgeGroup = form.AgeGroup,
                Year = form.Year,
                Description = form.Description,
                UserName = form.UserName,
                Image = imageBytes
            };

            var updated = await service.UpdateMovie(movieId, dto, ct);

            return Results.Ok(updated);
        });

        // DELETE
        group.MapDelete("/{movieId:guid}", async (
            Guid movieId,
            IMovieService service,
            CancellationToken ct) =>
        {
            await service.DeleteMovie(movieId, ct);
            return Results.NoContent();
        });
    }
}