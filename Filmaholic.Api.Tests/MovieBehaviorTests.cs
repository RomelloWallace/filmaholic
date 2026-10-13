using Filmaholic.Api.Classes;
using Filmaholic.Api.Data;
using Filmaholic.Api.Endpoints;
using Filmaholic.Api.Entities;
using Filmaholic.Api.Requests;
using Filmaholic.Api.Validators;
using Filmaholic.Shared.Dtos;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Filmaholic.Api.Tests;

public sealed class MovieBehaviorTests
{
    [Fact]
    public async Task Create_validator_rejects_whitespace_title()
    {
        var validator = new CreateMovieValidator();
        var request = new CreateMovieRequest
        {
            Title = "   ",
            Genre = "Drama",
            AgeGroup = "Adult"
        };

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Title));
    }

    [Fact]
    public async Task Create_validator_allows_missing_optional_year()
    {
        var validator = new CreateMovieValidator();
        var request = new CreateMovieRequest
        {
            Title = "Arrival",
            Genre = "Science Fiction",
            AgeGroup = "Teen"
        };

        var result = await validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Update_validator_rejects_whitespace_only_values()
    {
        var validator = new UpdateMovieValidator();
        var request = new UpdateMovieRequest { Title = "   " };

        var result = await validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(request.Title));
    }

    [Fact]
    public async Task Add_movie_trims_title_before_persisting()
    {
        await using var fixture = await MovieDatabase.CreateAsync();

        var movie = await fixture.Service.AddMovie(
            new CreateMovieDto
            {
                Title = "  Arrival  ",
                Genre = "Science Fiction",
                AgeGroup = "Teen",
                Year = 2016
            },
            fixture.UserId,
            "test-user");

        Assert.Equal("Arrival", movie.Title);
        Assert.Equal("Arrival", await fixture.Context.Movies
            .Where(item => item.Id == movie.Id)
            .Select(item => item.Title)
            .SingleAsync());
    }

    [Fact]
    public async Task Add_movie_rejects_duplicate_title_and_year()
    {
        await using var fixture = await MovieDatabase.CreateAsync();
        var movie = new CreateMovieDto
        {
            Title = "Arrival",
            Genre = "Science Fiction",
            AgeGroup = "Teen",
            Year = 2016
        };

        await fixture.Service.AddMovie(movie, fixture.UserId, "test-user");

        await Assert.ThrowsAsync<ConflictException>(() => fixture.Service.AddMovie(
            new CreateMovieDto
            {
                Title = " Arrival ",
                Genre = "Drama",
                AgeGroup = "Adult",
                Year = 2016
            },
            fixture.UserId,
            "test-user"));
    }

    [Fact]
    public async Task Update_movie_rejects_duplicate_title_and_year()
    {
        await using var fixture = await MovieDatabase.CreateAsync();
        var first = await fixture.Service.AddMovie(
            new CreateMovieDto
            {
                Title = "Arrival",
                Genre = "Science Fiction",
                AgeGroup = "Teen",
                Year = 2016
            },
            fixture.UserId,
            "test-user");
        var second = await fixture.Service.AddMovie(
            new CreateMovieDto
            {
                Title = "Dune",
                Genre = "Science Fiction",
                AgeGroup = "Teen",
                Year = 2021
            },
            fixture.UserId,
            "test-user");

        await Assert.ThrowsAsync<ConflictException>(() => fixture.Service.UpdateMovie(
            second.Id,
            new UpdateMovieDto { Title = first.Title, Year = first.Year },
            fixture.UserId));
    }

    private sealed class MovieDatabase : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private MovieDatabase(SqliteConnection connection, MoviesDbContext context, Guid userId)
        {
            _connection = connection;
            Context = context;
            UserId = userId;
            Service = new MovieService(context);
        }

        public MoviesDbContext Context { get; }
        public MovieService Service { get; }
        public Guid UserId { get; }

        public static async Task<MovieDatabase> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new MoviesDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var userId = Guid.NewGuid();
            context.Users.Add(new UserEntity
            {
                Id = userId,
                UserName = "test-user",
                NormalizedUserName = "TEST-USER",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM"
            });
            await context.SaveChangesAsync();

            return new MovieDatabase(connection, context, userId);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
