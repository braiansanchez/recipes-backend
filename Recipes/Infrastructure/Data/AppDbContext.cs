using Microsoft.EntityFrameworkCore;
using Recipes.Core.Entities;

namespace Recipes.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed de Receta 1
        modelBuilder.Entity<Recipe>().HasData(new Recipe
        {
            Id = 1,
            Title = "Tacos al Pastor",
            Description = "Clásico mexicano",
            CookingTimeMinutes = 45,
            ImageUrl = "https://theeburgerdude.com/wp-content/uploads/2024/11/Al-Pastor-01-1024x1024.jpg",
            Instructions = "Marinar la carne y asar."
        });

        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Name = "Cerdo", Amount = 500, Unit = "gramos", RecipeId = 1 },
            new Ingredient { Id = 2, Name = "Piña", Amount = 1, Unit = "pieza", RecipeId = 1 }
        );

        // Seed de Receta 2
        modelBuilder.Entity<Recipe>().HasData(new Recipe
        {
            Id = 2,
            Title = "Pasta Carbonara",
            Description = "Receta original italiana",
            CookingTimeMinutes = 20,
            ImageUrl = "https://www.informacibo.it/wp-content/uploads/2018/04/carbonara.jpg",
            Instructions = "Mezclar huevo y queso."
        });

        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 3, Name = "Huevo", Amount = 3, Unit = "unidades", RecipeId = 2 },
            new Ingredient { Id = 4, Name = "Pecorino", Amount = 50, Unit = "gramos", RecipeId = 2 }
        );
    }

    //Almacenar las fechas de created y updated automaticamente
    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Recipe && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (Recipe)entry.Entity;
            entity.UpdatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}