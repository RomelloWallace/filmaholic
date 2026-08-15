using System.Net.Http.Json;
using Filmaholic.Shared.Dtos;

namespace Filmaholic.App.Services;

public sealed class AuthService
{
    private readonly HttpClient _http;
    private readonly TokenStore _tokenStore;

    public AuthService(HttpClient http, TokenStore tokenStore)
    {
        _http = http;
        _tokenStore = tokenStore;
    }

    public string? Token => _tokenStore.Token;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_tokenStore.Token);

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto login)
    {
        var response = await _http.PostAsJsonAsync("filmaholic/v1/auth/login", login);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (loginResponse is not null)
        {
            _tokenStore.Token = loginResponse.Token;
        }

        return loginResponse;
    }

    public void Logout()
    {
        _tokenStore.Token = null;
    }
}
