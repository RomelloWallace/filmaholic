using Filmaholic.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Filmaholic.Api.Data;
public class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }

    public DbSet<MovieModel> Movies => Set<MovieModel>();
}
