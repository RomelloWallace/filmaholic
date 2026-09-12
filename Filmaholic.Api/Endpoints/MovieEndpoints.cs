using Filmaholic.Shared.Dtos;
using Filmaholic.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Filmaholic.Api.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Filmaholic.Api.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("filmaholic/v1/movies")
            .RequireAuthorization();

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
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var (userId, userName) = GetAuthenticatedUser(httpContext);
            byte[]? imageBytes = null;
            const long maxBytes = 10 * 1024 * 1024; // 10 MB

            if (form.Image is not null)
            {
                if (form.Image.Length > maxBytes)
                    return Results.BadRequest(new { Error = "Image too large" });

                try
                {
                    await using var ms = new MemoryStream();
                    await form.Image.CopyToAsync(ms, ct);
                    imageBytes = ms.ToArray();
                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            var request = new CreateMovieDto
            {
                Title = form.Title,
                Genre = form.Genre,
                AgeGroup = form.AgeGroup,
                Year = form.Year,
                Description = form.Description,
                Image = imageBytes
            };
            var movie = await service.AddMovie(request, userId, userName, ct);

            return TypedResults.Created(
                $"/filmaholic/v1/movies/{movie.Id}",
                movie);
        }).DisableAntiforgery();

        // UPDATE (PATCH)
        group.MapPatch("/{movieId:guid}/edit", async (
            Guid movieId,
            [FromForm] UpdateMovieRequest form,
            IMovieService service,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var (userId, _) = GetAuthenticatedUser(httpContext);
            byte[]? imageBytes = null;
            const long maxBytes = 10 * 1024 * 1024; // 10 MB

            if (form.Image is not null)
            {
                if (form.Image.Length > maxBytes)
                    return Results.BadRequest(new { Error = "Image too large" });

                try
                {
                    await using var ms = new MemoryStream();
                    await form.Image.CopyToAsync(ms, ct);
                    imageBytes = ms.ToArray();
                }
                catch (Exception)
                {
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            var dto = new UpdateMovieDto
            {
                Title = form.Title,
                Genre = form.Genre,
                AgeGroup = form.AgeGroup,
                Year = form.Year,
                Description = form.Description,
                Image = imageBytes
            };

            var updated = await service.UpdateMovie(movieId, dto, userId, ct);

            return Results.Ok(updated);
        }).DisableAntiforgery();

        // DELETE
        group.MapDelete("/{movieId:guid}", async (
            Guid movieId,
            IMovieService service,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var (userId, _) = GetAuthenticatedUser(httpContext);
            await service.DeleteMovie(movieId, userId, ct);
            return Results.NoContent();
        });
    }

    private static (Guid UserId, string UserName) GetAuthenticatedUser(HttpContext httpContext)
    {
        var userIdValue = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException("The authenticated user identifier is invalid.");

        var userName = httpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? httpContext.User.FindFirstValue(JwtRegisteredClaimNames.UniqueName)
            ?? throw new UnauthorizedAccessException("The authenticated username is missing.");

        return (userId, userName);
    }
}