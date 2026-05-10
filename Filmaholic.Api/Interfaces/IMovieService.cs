using Filmaholic.Shared.Dtos;

namespace Filmaholic.Api.Interfaces;

public interface IMovieService
{
    Task<GetMovieDto> AddMovie(CreateMovieDto newMovie, CancellationToken ct = default);
    Task<GetMovieDto> GetMovieById(Guid movieId, CancellationToken ct = default);
    Task<IEnumerable<GetMoviesDto>> GetAllMovies(CancellationToken ct = default);
    Task<GetMovieDto> UpdateMovie(Guid movieId, UpdateMovieDto update, CancellationToken ct = default);
    Task DeleteMovie(Guid movieId, CancellationToken ct = default);
}