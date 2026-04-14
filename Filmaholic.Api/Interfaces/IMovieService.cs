using Filmaholic.Shared.Dtos;

namespace Filmaholic.Api.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<GetMoviesDto>> GetAllMovies();
    Task<GetMovieDto?> GetMovieById(Guid id);
    Task<GetMovieDto> AddMovie(CreateMovieDto dto);
    Task<GetMovieDto?> UpdateMovie(Guid id, UpdateMovieDto dto);
    Task<bool> DeleteMovie(Guid id);
}