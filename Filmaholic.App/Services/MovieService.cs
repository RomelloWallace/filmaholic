using System.Net.Http.Json;
using Filmaholic.Shared.Dtos;

namespace Filmaholic.App.Services;

public class MovieService
{
    private readonly HttpClient _http;

    public MovieService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<GetMovieDto>> GetMoviesAsync()
    {
        var url = "filmaholic/v1/movies/";
        Console.WriteLine($"Calling: {_http.BaseAddress}{url}");

        var result = await _http.GetFromJsonAsync<List<GetMovieDto>>(url);
        return result ?? new List<GetMovieDto>();
    }

    public async Task<GetMovieDto?> GetMovieAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<GetMovieDto>($"filmaholic/v1/movies/{id}");
    }

    public async Task DeleteMovieAsync(Guid id)
    {
        await _http.DeleteAsync($"filmaholic/v1/movies/{id}");
    }
}