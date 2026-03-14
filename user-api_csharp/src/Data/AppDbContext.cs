using Microsoft.EntityFrameworkCore;
using user_api_csharp.src.Models;

namespace user_api_csharp.src.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<User> Users => Set<User>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Supplier> Suppliers => Set<Supplier>();
  public DbSet<Category> Categories => Set<Category>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
      .HasIndex(u => u.Email)
      .IsUnique();

    modelBuilder.Entity<Product>()
      .Property(p => p.Price)
      .HasPrecision(18, 2);

    modelBuilder.Entity<Product>()
      .HasOne(p => p.Supplier)
      .WithMany(s => s.Products)
      .HasForeignKey(p => p.SupplierId)
      .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Product>()
      .HasOne(p => p.Category)
      .WithMany(c => c.Products)
      .HasForeignKey(p => p.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);

    base.OnModelCreating(modelBuilder);
  }
}
