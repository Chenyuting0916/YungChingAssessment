using Microsoft.EntityFrameworkCore;
using YungChingAssessment.Core.Entities;

namespace YungChingAssessment.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed data (Optional, but good for testing)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
            new Category { Id = 2, Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Chai", Price = 18.00m, CategoryId = 1, IsActive = true },
            new Product { Id = 2, Name = "Chang", Price = 19.00m, CategoryId = 1, IsActive = true },
            new Product { Id = 3, Name = "Aniseed Syrup", Price = 10.00m, CategoryId = 2, IsActive = true }
        );
    }
}
