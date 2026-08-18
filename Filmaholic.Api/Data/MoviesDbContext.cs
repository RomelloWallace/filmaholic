using Filmaholic.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmaholic.Api.Data;

public class MoviesDbContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
    {
    }

    public DbSet<MovieEntity> Movies => Set<MovieEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<MovieEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        });
    }
}
