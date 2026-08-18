using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Filmaholic.Api.Entities;
using Filmaholic.Api.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Filmaholic.Api.Interfaces;

namespace Filmaholic.Api.Services;

public sealed class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;

    public AuthService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<(bool Success, string[] Errors)> RegisterAsync(
        string userName,
        string email,
        string password,
        CancellationToken ct = default)
    {
        var user = new UserEntity
        {
            UserName = userName,
            Email = email,
            NormalizedUserName = userName.ToUpper(),
            NormalizedEmail = email.ToUpper()
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description).ToArray());
        }

        return (true, Array.Empty<string>());
    }

    public async Task<(UserEntity? User, bool Success, string[] Errors)> LoginAsync(
        string userName,
        string password,
        CancellationToken ct = default)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null || !user.IsActive)
        {
            return (null, false, new[] { "Invalid username or password" });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
        {
            return (null, false, new[] { "Invalid username or password" });
        }

        return (user, true, Array.Empty<string>());
    }

    public string CreateJwtToken(UserEntity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
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

