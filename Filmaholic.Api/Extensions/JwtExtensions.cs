using Filmaholic.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Filmaholic.Api.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        services.AddOptions<JwtSettings>()
            .Bind(jwtSection)
            .Validate(settings => Encoding.UTF8.GetByteCount(settings.Key) >= 32,
                "Jwt:Key must contain at least 32 bytes.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer),
                "Jwt:Issuer is required.")
            .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience),
                "Jwt:Audience is required.")
            .ValidateOnStart();

        var jwtSettings = jwtSection.Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt configuration is required.");

        var keyBytes = Encoding.UTF8.GetBytes(jwtSettings.Key);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = signingKey
            };
        });

        return services;
    }
}
