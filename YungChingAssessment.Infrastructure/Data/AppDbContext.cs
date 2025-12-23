using Microsoft.EntityFrameworkCore;
using YungChingAssessment.Core.Entities;

namespace YungChingAssessment.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Chai", Price = 18.00m, IsActive = true },
            new Product { Id = 2, Name = "Chang", Price = 19.00m, IsActive = true },
            new Product { Id = 3, Name = "Aniseed Syrup", Price = 10.00m, IsActive = true }
        );
    }
}
