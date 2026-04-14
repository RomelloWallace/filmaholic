using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Entities;
using Filmaholic.Api.Data;
using Microsoft.EntityFrameworkCore;
using Filmaholic.Shared.Dtos;

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
        public async Task<GetMovieDto> AddMovie(CreateMovieDto newMovie)
        {
            var createdMovie = new MovieEntity{
                Genre = newMovie.Genre,
                Title = newMovie.Title,
                Year = newMovie.Year,
                AgeGroup = newMovie.AgeGroup,
                Description = newMovie.Description,
                UserName = newMovie.UserName
            };
            await _dbContext.Movies.AddAsync(createdMovie);
            await _dbContext.SaveChangesAsync();
            return MapSingleMovieToRecord(createdMovie);
        }

        public async Task<bool> DeleteMovie(Guid movieId)
        {
            var movieToRemove = _dbContext.Movies.Where(movies => movies.Id == movieId ).FirstOrDefault();
            if(movieToRemove == null){ return false; }
            _dbContext.Movies.Remove(movieToRemove);
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<IEnumerable<GetMoviesDto>> GetAllMovies()
        {  
            var movies = await _dbContext.Movies.AsNoTracking().Select(
                movie => new GetMoviesDto{
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
                .ToListAsync();
            return movies;
        }

        public async Task<GetMovieDto?> GetMovieById(Guid movieId)
        {   var movie = await _dbContext.Movies.AsNoTracking().FirstOrDefaultAsync(movie => movie.Id == movieId);
            if (movie == null) return null;
            return MapSingleMovieToRecord(movie);
        }

        public async Task<GetMovieDto?> UpdateMovie(Guid movieId, UpdateMovieDto update)
        {
            var movie = await _dbContext.Movies.FindAsync(movieId);

            if (movie is null)
                return null;

            if (update.Title is not null)
                movie.Title = update.Title;

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

            await _dbContext.SaveChangesAsync();

            return MapSingleMovieToRecord(movie);
        }
    }

}