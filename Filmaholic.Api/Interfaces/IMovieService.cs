using Filmaholic.Shared.Dtos;

namespace Filmaholic.Api.Interfaces;

public interface IMovieService
{
    Task<GetMovieDto> AddMovie(CreateMovieDto newMovie, Guid userId, string userName, CancellationToken ct = default);
    Task<GetMovieDto> GetMovieById(Guid movieId, CancellationToken ct = default);
    Task<IEnumerable<GetMoviesDto>> GetAllMovies(CancellationToken ct = default);
    Task<GetMovieDto> UpdateMovie(Guid movieId, UpdateMovieDto update, Guid userId, CancellationToken ct = default);
    Task DeleteMovie(Guid movieId, Guid userId, CancellationToken ct = default);
}