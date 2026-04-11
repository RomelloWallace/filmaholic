using Filmaholic.Api.Records;

namespace Filmaholic.Api.Interfaces;

public interface IMovie
{
    Task<IEnumerable<GetMoviesRecord>> GetAllMovies();
    Task<GetMovieRecord?> GetMovieById(Guid id);
    Task<GetMovieRecord> AddMovie(CreateMovieRecord dto);
    Task<GetMovieRecord?> UpdateMovie(Guid id, UpdateMovieRecord dto);
    Task<bool> DeleteMovie(Guid id);
}