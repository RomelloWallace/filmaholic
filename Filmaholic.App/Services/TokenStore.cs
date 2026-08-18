namespace Filmaholic.App.Services;

public sealed class TokenStore
{
    private const string TokenKey = "auth_token";

    public async Task<string?> GetTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }

    public async Task StoreTokenAsync(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
    }

    public void ClearToken()
    {
        SecureStorage.Remove(TokenKey);
    }
}