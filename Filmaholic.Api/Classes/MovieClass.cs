using Filmaholic.Api.Interfaces;
using Filmaholic.Api.DbModels;
using Filmaholic.Api.Data;
using Microsoft.EntityFrameworkCore;
using Filmaholic.Shared.Records;

namespace Filmaholic.Api.Classes
{
    public class MovieClass : IMovie
    {
        private readonly MoviesDbContext _dbContext;

         private static GetMovieRecord MapSingleMovieToRecord(MovieModel movie)
        {
            return new GetMovieRecord(
                movie.Id,
                movie.Title,
                movie.Genre ,
                movie.Year,
                movie.Description,
                movie.AgeGroup,
                movie.UserName,
                movie.AddedAt,
                movie.UpdatedAt,
                movie.Image
            );
        }
        private static GetMoviesRecord MapManyMoviesToRecord(MovieModel movie)
        {
            return new GetMoviesRecord(
                movie.Id,
                movie.Title,
                movie.Genre ,
                movie.Year,
                movie.Description,
                movie.AgeGroup,
                movie.UserName,
                movie.AddedAt,
                movie.UpdatedAt
            );
        }

        public MovieClass(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetMovieRecord> AddMovie(CreateMovieRecord newMovie)
        {
            var createdMovie = new MovieModel{
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

        public async Task<IEnumerable<GetMoviesRecord>> GetAllMovies()
        {  
            IEnumerable<MovieModel> movies = await _dbContext.Movies.AsNoTracking().ToListAsync();
            var movieRecords = movies.Select(movie => MapManyMoviesToRecord(movie)).ToList();
            return movieRecords;
        }

        public async Task<GetMovieRecord?> GetMovieById(Guid movieId)
        {   var movie = await _dbContext.Movies.AsNoTracking().FirstOrDefaultAsync(movie => movie.Id == movieId);
            if (movie == null) return null;
            return MapSingleMovieToRecord(movie);
        }

        public async Task<GetMovieRecord?> UpdateMovie(Guid movieId, UpdateMovieRecord update)
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