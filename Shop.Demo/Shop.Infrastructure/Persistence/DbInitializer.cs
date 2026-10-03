using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistence;

public static class DbInitializer
{
    // Demo only: creates the schema (no migrations) and inserts sample data once.
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (await context.Categories.AnyAsync())
            return;

        var electronics = new Category { Name = "Electronics" };
        var books = new Category { Name = "Books" };

        context.Products.AddRange(
            new Product { Name = "Laptop", Price = 1200m, Stock = 10, Category = electronics },
            new Product { Name = "Phone", Price = 800m, Stock = 25, Category = electronics },
            new Product { Name = "Clean Architecture (book)", Price = 35m, Stock = 50, Category = books });

        await context.SaveChangesAsync();
    }
}
