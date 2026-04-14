using Filmaholic.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Filmaholic.Api.Data;
public class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }

    public DbSet<MovieEntity> Movies => Set<MovieEntity>();
}
