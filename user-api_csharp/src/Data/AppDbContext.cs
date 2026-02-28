using Microsoft.EntityFrameworkCore;
using user_api_csharp.Models;

namespace user_api_csharp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Correo)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
