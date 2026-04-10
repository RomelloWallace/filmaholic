using Filmaholic.Api.Dtos;
using Filmaholic.Api.Models;

namespace Filmaholic.Api.Interfaces
{
    public interface IMovie
    { 
        Task<IEnumerable<MovieModel>> GetAllMovies();
        Task<MovieModel?> GetMovieById(Guid movieId);
        Task<MovieModel> AddMovie(CreateMovieDto movie);
        Task<MovieModel?> UpdateMovie(Guid movieId, UpdateMovieDto movie);
        Task<bool> DeleteMovie(Guid movieId);
    }
}