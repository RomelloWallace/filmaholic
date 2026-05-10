using Filmaholic.Api.Entities;
using Filmaholic.Api.Data;
using Microsoft.EntityFrameworkCore;
using Filmaholic.Shared.Dtos;
using Filmaholic.Api.Endpoints;
using Filmaholic.Api.Interfaces;

namespace Filmaholic.Api.Classes
{
    public class MovieService : IMovieService
    {
        private readonly MoviesDbContext _dbContext;

        private static GetMovieDto MapSingleMovieToRecord(MovieEntity movie)
        {
            return new GetMovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year,
                Description = movie.Description,
                AgeGroup = movie.AgeGroup,
                UserName = movie.UserName,
                AddedAt = movie.AddedAt,
                UpdatedAt = movie.UpdatedAt,
                Image = movie.Image
            };
        }
        private static GetMoviesDto MapManyMoviesToRecord(MovieEntity movie)
        {
            return new GetMoviesDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year,
                Description = movie.Description,
                AgeGroup = movie.AgeGroup,
                UserName = movie.UserName,
                AddedAt = movie.AddedAt,
                UpdatedAt = movie.UpdatedAt
            };
        }

        public MovieService(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetMovieDto> AddMovie(CreateMovieDto newMovie, CancellationToken ct = default)
        {
            var exists = await _dbContext.Movies
                .AnyAsync(m =>
                    m.Title == newMovie.Title &&
                    m.Year == newMovie.Year,
                    ct);

            if (exists)
                throw new ConflictException("A movie with the same title and year already exists.");

            var createdMovie = new MovieEntity
            {
                Id = Guid.NewGuid(),
                Title = newMovie.Title.Trim(),
                Genre = newMovie.Genre,
                Year = newMovie.Year,
                AgeGroup = newMovie.AgeGroup,
                Description = newMovie.Description,
                UserName = newMovie.UserName,
                Image = newMovie.Image
            };

            await _dbContext.Movies.AddAsync(createdMovie, ct);
            await _dbContext.SaveChangesAsync(ct);

            return MapSingleMovieToRecord(createdMovie);
        }

        public async Task DeleteMovie(Guid movieId, CancellationToken ct = default)
        {
            var movie = await _dbContext.Movies
                .FirstOrDefaultAsync(m => m.Id == movieId, ct);

            if (movie is null)
                throw new NotFoundException($"Movie with id '{movieId}' was not found.");

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<GetMoviesDto>> GetAllMovies(CancellationToken ct = default)
        {
            var movies = await _dbContext.Movies
                .AsNoTracking()
                .Select(movie => new GetMoviesDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Genre = movie.Genre,
                    Year = movie.Year,
                    Description = movie.Description,
                    AgeGroup = movie.AgeGroup,
                    UserName = movie.UserName,
                    AddedAt = movie.AddedAt,
                    UpdatedAt = movie.UpdatedAt
                })
                .ToListAsync(ct);

            return movies;
        }

        public async Task<GetMovieDto> GetMovieById(Guid movieId, CancellationToken ct = default)
        {
            var movie = await _dbContext.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == movieId, ct);

            if (movie is null)
                throw new NotFoundException($"Movie {movieId} not found");

            return MapSingleMovieToRecord(movie);
        }

        public async Task<GetMovieDto> UpdateMovie(Guid movieId, UpdateMovieDto update, CancellationToken ct = default)
        {
            var movie = await _dbContext.Movies.FindAsync([movieId], ct);

            if (movie is null)
                throw new NotFoundException($"Movie with id '{movieId}' was not found.");

            if (update.Title is not null)
                movie.Title = update.Title.Trim();

            if (update.Genre is not null)
                movie.Genre = update.Genre;

            if (update.Description is not null)
                movie.Description = update.Description;

            if (update.Year is not null)
                movie.Year = update.Year;

            if (update.AgeGroup is not null)
                movie.AgeGroup = update.AgeGroup;

            if (update.UserName is not null)
                movie.UserName = update.UserName;

            movie.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(ct);

            return MapSingleMovieToRecord(movie);
        }
    }
}