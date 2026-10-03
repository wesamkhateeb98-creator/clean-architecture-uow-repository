using Microsoft.EntityFrameworkCore;
using Shop.Application.Abstracts.Repositories;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
{
    public Task<List<Category>> GetAllAsync(CancellationToken cancellationToken) =>
        DbContext.Set<Category>()
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);

    public async Task EnsureExistsAsync(int id, CancellationToken cancellationToken)
    {
        var exists = await ExistsByIdAsync(id, cancellationToken);

        if (!exists)
            throw new NotFoundException($"Category {id} not found.");
    }

    public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken) =>
        DbContext.Set<Category>().AnyAsync(c => c.Name == name, cancellationToken);
}
