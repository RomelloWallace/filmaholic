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
        group.MapGet("/", async (IMovieService service) =>
        {
            var movies = await service.GetAllMovies();
            return Results.Ok(movies);
        })
        .Produces<IEnumerable<GetMoviesDto>>(StatusCodes.Status200OK);

        // GET BY ID
        group.MapGet("/{movieId:guid}", async (Guid movieId, IMovieService service) =>
        {
            var movie = await service.GetMovieById(movieId);

            return movie is null
                ? Results.NotFound()
                : Results.Ok(movie);
        })
        .Produces<GetMovieDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // CREATE
        group.MapPost("/", async (
            [FromForm] CreateMovieRequest form,
            IMovieService service) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var dto = new CreateMovieDto(
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
        .Accepts<CreateMovieRequest>("multipart/form-data")
        .Produces<GetMovieDto>(StatusCodes.Status201Created)
        .DisableAntiforgery();

        // UPDATE (PATCH)
        group.MapPatch("/{movieId:guid}", async (
            Guid movieId,
            [FromForm] UpdateMovieRequest form,
            IMovieService service) =>
        {
            byte[]? imageBytes = null;

            if (form.Image is not null)
            {
                using var ms = new MemoryStream();
                await form.Image.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var dto = new UpdateMovieDto(
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
        .Accepts<UpdateMovieRequest>("multipart/form-data")
        .Produces<GetMovieDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .DisableAntiforgery();

        // DELETE
        group.MapDelete("/{movieId:guid}", async (Guid movieId, IMovieService service) =>
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