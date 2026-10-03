using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public Task<bool> ExistsAsync(int id, CancellationToken ct = default) =>
        Context.Categories.AnyAsync(c => c.Id == id, ct);

    public Task<bool> NameExistsAsync(string name, CancellationToken ct = default) =>
        Context.Categories.AnyAsync(c => c.Name == name, ct);
}
