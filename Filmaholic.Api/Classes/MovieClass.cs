using Filmaholic.Api.Interfaces;
using Filmaholic.Api.Models;
using Filmaholic.Api.Data;
using Microsoft.EntityFrameworkCore;
using Filmaholic.Api.Dtos;

namespace Filmaholic.Api.Classes
{
    public class MovieClass : IMovie
    {
        private readonly MoviesDbContext _dbContext;
        public MovieClass(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<MovieModel> AddMovie(CreateMovieDto newMovie)
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
            return createdMovie;
        }

        public async Task<bool> DeleteMovie(Guid movieId)
        {
            var movieToRemove = _dbContext.Movies.Where(movies => movies.Id == movieId ).FirstOrDefault();
            if(movieToRemove == null){ return false; }
            _dbContext.Movies.Remove(movieToRemove);
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<IEnumerable<MovieModel>> GetAllMovies()
        {
            IEnumerable<MovieModel> movies = await _dbContext.Movies.AsNoTracking().ToListAsync();
            return movies;
        }

        public async Task<MovieModel?> GetMovieById(Guid movieId)
        {   
            return await _dbContext.Movies.AsNoTracking().FirstOrDefaultAsync(movie => movie.Id == movieId);
        }

        public async Task<MovieModel?> UpdateMovie(Guid movieId, UpdateMovieDto movie)
        {
            var movieToUpdate = await _dbContext.Movies.FindAsync(movieId );
            if(movieToUpdate == null){return null;}
            movieToUpdate.Title = movie.Title;
            movieToUpdate.Genre = movie.Genre;
            movieToUpdate.Description = movie.Description;
            movieToUpdate.Year = movie.Year;
            movieToUpdate.AgeGroup = movie.AgeGroup;
            movieToUpdate.UpdatedAt = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return movieToUpdate;
        }
    }

}