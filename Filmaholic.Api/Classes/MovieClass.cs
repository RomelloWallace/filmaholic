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
                Description = newMovie.Description,
                UserName = newMovie.UserName
            };
            await _dbContext.Movies.AddAsync(createdMovie);
            _dbContext.SaveChanges();
            return createdMovie;
        }

        public Task<bool> DeleteMovie(Guid movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieModel>> GetAllMovies()
        {
            IEnumerable<MovieModel> movies = await _dbContext.Movies.AsNoTracking().ToListAsync();
            return movies;
        }

        public Task<MovieModel?> GetMovieById(Guid movieId)
        {
            throw new NotImplementedException();
        }

        public Task<MovieModel?> UpdateMovie(Guid movieId, UpdateMovieDto movie)
        {
            throw new NotImplementedException();
        }
    }

}