using Filmaholic.Shared.Dtos;
using Filmaholic.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Filmaholic.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("filmaholic/v1/auth")
            .WithTags("Authentication");

        group.MapPost("/register", async (
            [FromBody] RegisterRequestDto register,
            IAuthService authService,
            CancellationToken ct) =>
        {
            if (register.Password != register.ConfirmPassword)
            {
                return Results.BadRequest(new { Error = "Passwords do not match" });
            }

            var (success, errors) = await authService.RegisterAsync(
                register.UserName,
                register.Email,
                register.Password,
                ct);

            if (!success)
            {
                return Results.BadRequest(new { Errors = errors });
            }

            return Results.Created($"/filmaholic/v1/auth/register", new { Message = "User registered successfully" });
        })
        .WithName("Register");

        group.MapPost("/login", async (
            [FromBody] LoginRequestDto login,
            IAuthService authService,
            CancellationToken ct) =>
        {
            var (user, success, errors) = await authService.LoginAsync(
                login.UserName,
                login.Password,
                ct);

            if (!success || user == null)
            {
                return Results.Unauthorized();
            }

            var token = authService.CreateJwtToken(user);

            return Results.Ok(new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        })
        .WithName("Login");
    }
}
