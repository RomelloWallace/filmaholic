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

        group.MapPost("/login", async (
            [FromBody] LoginRequestDto login,
            IAuthService authService) =>
        {
            var isValid = await authService.ValidateCredentialsAsync(login.UserName, login.Password);

            if (!isValid)
            {
                return Results.Unauthorized();
            }

            var token = authService.CreateJwtToken(login.UserName);

            return Results.Ok(new LoginResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });
        })
        .WithName("Login");
    }
}
