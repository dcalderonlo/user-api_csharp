using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users => Set<User>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
      .HasIndex(u => u.Email)
      .IsUnique();

    base.OnModelCreating(modelBuilder);
  }
}
