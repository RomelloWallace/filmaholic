using System.Net;
using System.Net.Http.Json;
using Filmaholic.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace Filmaholic.App.Services;

public class MovieService
{
    private readonly HttpClient _http;
    private readonly NavigationManager _nav;

    public MovieService(HttpClient http, NavigationManager nav)
    {
        _http = http;
        _nav = nav;
    }

    public async Task<List<GetMovieDto>> GetMoviesAsync()
    {
        var url = "filmaholic/v1/movies/";
        Console.WriteLine($"Calling: {_http.BaseAddress}{url}");

        var response = await _http.GetAsync(url);
        await RedirectOnAuthFailure(response.StatusCode);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<GetMovieDto>>() ?? new List<GetMovieDto>();
    }

    public async Task<GetMovieDto?> GetMovieAsync(Guid id)
    {
        var response = await _http.GetAsync($"filmaholic/v1/movies/{id}");
        await RedirectOnAuthFailure(response.StatusCode);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GetMovieDto>();
    }

    public async Task DeleteMovieAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"filmaholic/v1/movies/{id}");
        await RedirectOnAuthFailure(response.StatusCode);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateMovieAsync(UpdateMovieDto movie)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(movie.Id ?? ""), "Id");
        content.Add(new StringContent(movie.Title ?? ""), "Title");
        content.Add(new StringContent(movie.Genre ?? ""), "Genre");
        content.Add(new StringContent(movie.AgeGroup ?? ""), "AgeGroup");
        content.Add(new StringContent(movie.Description ?? ""), "Description");
        if (movie.Year.HasValue)
        {
            content.Add(new StringContent(movie.Year.Value.ToString()), "Year");
        }
        if (movie.Image != null)
        {
            	var imageContent = new ByteArrayContent(movie.Image);
            	imageContent.Headers.ContentType =
            	    new MediaTypeHeaderValue("application/octet-stream");
            	imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            	{
            	    Name = "\"Image\"",
            	    FileName = "\"image.jpg\""
            	};

            	content.Add(imageContent, "Image", "image.jpg");
        }
        

        var response = await _http.PatchAsync($"filmaholic/v1/movies/{movie.Id}/edit", content);
        await RedirectOnAuthFailure(response.StatusCode);

        response.EnsureSuccessStatusCode();
    }

    public async Task AddMovieAsync(CreateMovieDto movie)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(movie.Title ?? ""), "Title");
        content.Add(new StringContent(movie.Genre ?? ""), "Genre");
        content.Add(new StringContent(movie.AgeGroup ?? ""), "AgeGroup");
        content.Add(new StringContent(movie.Description ?? ""), "Description");
        if (movie.Year.HasValue)
        {
            content.Add(new StringContent(movie.Year.Value.ToString()), "Year");
        }
        if (movie.Image != null)
        {
            	var imageContent = new ByteArrayContent(movie.Image);
            	imageContent.Headers.ContentType =
            	    new MediaTypeHeaderValue("application/octet-stream");
            	imageContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            	{
            	    Name = "\"Image\"",
            	    FileName = "\"image.jpg\""
            	};

            	content.Add(imageContent, "Image", "image.jpg");
        }
        

        var response = await _http.PostAsync("filmaholic/v1/movies/", content);
        await RedirectOnAuthFailure(response.StatusCode);

        response.EnsureSuccessStatusCode();
    }

    private Task RedirectOnAuthFailure(HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden)
        {
            _nav.NavigateTo("/login");
        }

        return Task.CompletedTask;
    }
}