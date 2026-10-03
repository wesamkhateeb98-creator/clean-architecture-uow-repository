using Microsoft.EntityFrameworkCore;
using Shop.Application.Exceptions;
using Shop.Application.Abstract.Repositories;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
    public async Task EnsureExistsAsync(int id, CancellationToken ct = default)
    {
        if (!await Context.Categories.AnyAsync(c => c.Id == id, ct))
            throw new BadRequestException($"Category {id} does not exist.");
    }

    public Task<bool> NameExistsAsync(string name, CancellationToken ct = default) =>
        Context.Categories.AnyAsync(c => c.Name == name, ct);
}
