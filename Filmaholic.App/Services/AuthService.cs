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

    public async Task<string?> GetTokenAsync()
    {
        return await _tokenStore.GetTokenAsync();
    }

    public async Task<bool> isAuthenticatedAsyncAsync()
    {
        var token = await _tokenStore.GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<(LoginResponseDto? Response, string? Error)> LoginAsync(LoginRequestDto login)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("filmaholic/v1/auth/login", login);
        
            if (!response.IsSuccessStatusCode)
                return (null, "Invalid credentials");
        
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (loginResponse is not null)
            {
                await _tokenStore.StoreTokenAsync(loginResponse.Token);  // await the async method
            }
        
            return (loginResponse, null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public Task LogoutAsync()
    {
        _tokenStore.ClearToken();
        return Task.CompletedTask;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequestDto register)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("filmaholic/v1/auth/register", register);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return (false, "Registration failed. Username or email may already exist.");
            }
            
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
