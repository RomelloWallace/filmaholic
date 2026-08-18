using Filmaholic.Api.Entities;

namespace Filmaholic.Api.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string[] Errors)> RegisterAsync(string userName, string email, string password, CancellationToken ct = default);
    Task<(UserEntity? User, bool Success, string[] Errors)> LoginAsync(string userName, string password, CancellationToken ct = default);
    string CreateJwtToken(UserEntity user);
}
