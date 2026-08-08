namespace Filmaholic.Api.Interfaces;

public interface IAuthService
{
    Task<bool> ValidateCredentialsAsync(string userName, string password, CancellationToken ct = default);
    string CreateJwtToken(string userName);
}
