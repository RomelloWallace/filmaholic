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

    public async Task UpdateMovieAsync(UpdateMovieDto movie)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(movie.Id ?? ""), "Id");
        content.Add(new StringContent(movie.Title ?? ""), "Title");
        content.Add(new StringContent(movie.Genre ?? ""), "Genre");
        content.Add(new StringContent(movie.AgeGroup ?? ""), "AgeGroup");
        content.Add(new StringContent(movie.UserName ?? ""), "UserName");
        content.Add(new StringContent(movie.Description ?? ""), "Description");
        if (movie.Year.HasValue)
        {
            content.Add(new StringContent(movie.Year.Value.ToString()), "Year");
        }
        if (movie.Image != null)
        {
            var imageContent = new ByteArrayContent(movie.Image);
            imageContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            content.Add(imageContent, "Image", "image.jpg");
        }
        

        var response = await _http.PatchAsync($"filmaholic/v1/movies/{movie.Id}/edit", content);

        response.EnsureSuccessStatusCode();
    }

        public async Task AddMovieAsync(CreateMovieDto movie)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(movie.Title ?? ""), "Title");
        content.Add(new StringContent(movie.Genre ?? ""), "Genre");
        content.Add(new StringContent(movie.AgeGroup ?? ""), "AgeGroup");
        content.Add(new StringContent(movie.UserName ?? ""), "UserName");
        content.Add(new StringContent(movie.Description ?? ""), "Description");
        if (movie.Year.HasValue)
        {
            content.Add(new StringContent(movie.Year.Value.ToString()), "Year");
        }
        if (movie.Image != null)
        {
            var imageContent = new ByteArrayContent(movie.Image);
            imageContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            content.Add(imageContent, "Image", "image.jpg");
        }
        

        var response = await _http.PostAsync("filmaholic/v1/movies/", content);

        response.EnsureSuccessStatusCode();
    }
}