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

        var hasData = await context.Set<Category>().AnyAsync();
        if (hasData)
            return;

        var electronics = Category.Create("Electronics");
        var books = Category.Create("Books");
        context.AddRange(electronics, books);
        await context.SaveChangesAsync();

        context.AddRange(
            Product.Create("Laptop", 1200m, 10, electronics.Id),
            Product.Create("Phone", 800m, 25, electronics.Id),
            Product.Create("Clean Architecture (book)", 35m, 50, books.Id));
        await context.SaveChangesAsync();
    }
}
