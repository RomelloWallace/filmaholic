using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Filmaholic.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Filmaholic.Api.Interfaces;

namespace Filmaholic.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken ct = default)
    {
        return Task.FromResult(userName == "testuser" && password == "password123");
    }

    public string CreateJwtToken(string userName)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
