using System.Net.Http.Json;
using Filmaholic.Shared.Dtos;

namespace Filmaholic.App.Services;

public sealed class AuthService
{
    private readonly HttpClient _http;
    private string? _jwtToken;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public string? Token => _jwtToken;

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_jwtToken);

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
            _jwtToken = loginResponse.Token;
        }

        return loginResponse;
    }

    public void Logout()
    {
        _jwtToken = null;
    }
}
