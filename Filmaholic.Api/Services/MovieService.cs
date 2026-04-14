using Filmaholic.Shared.Records;
using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Filmaholic.Api.Services;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("filmaholic/v1/movies");

        // GET ALL
        group.MapGet("/", async (IMovie service) =>
        {
            var movies = await service.GetAllMovies();
            return Results.Ok(movies);
        })
        .Produces<IEnumerable<GetMoviesRecord>>(StatusCodes.Status200OK);

        // GET BY ID
        group.MapGet("/{movieId:guid}", async (Guid movieId, IMovie service) =>
        {
            var movie = await service.GetMovieById(movieId);

            return movie is null
                ? Results.NotFound()
                : Results.Ok(movie);
        })
        .Produces<GetMovieRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // CREATE
        group.MapPost("/", async (
            [FromForm] CreateMovieForm form,
            IMovie service) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var dto = new CreateMovieRecord(
                form.Title,
                form.Genre,
                form.AgeGroup,
                form.UserName,
                form.Year,
                form.Description,
                imageBytes
            );

            var movie = await service.AddMovie(dto);

            return Results.Created(
                $"filmaholic/v1/movies/{movie.Id}",
                movie);
        })
        .Accepts<CreateMovieForm>("multipart/form-data")
        .Produces<GetMovieRecord>(StatusCodes.Status201Created)
        .DisableAntiforgery();

        // UPDATE (PATCH)
        group.MapPatch("/{movieId:guid}", async (
            Guid movieId,
            [FromForm] UpdateMovieForm form,
            IMovie service) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var dto = new UpdateMovieRecord(
                form.Title,
                form.Genre,
                form.AgeGroup,
                form.Year,
                form.Description,
                form.UserName,
                imageBytes
            );

            var updated = await service.UpdateMovie(movieId, dto);

            return updated is null
                ? Results.NotFound()
                : Results.Ok(updated);
        })
        .Accepts<UpdateMovieForm>("multipart/form-data")
        .Produces<GetMovieRecord>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .DisableAntiforgery();

        // DELETE
        group.MapDelete("/{movieId:guid}", async (Guid movieId, IMovie service) =>
        {
            var deleted = await service.DeleteMovie(movieId);

            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}