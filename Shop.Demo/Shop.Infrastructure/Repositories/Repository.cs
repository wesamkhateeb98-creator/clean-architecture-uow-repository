using Microsoft.EntityFrameworkCore;
using Shop.Application.Abstracts;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

// Generic implementation shared by all repositories.
// It only stages changes in the DbContext -- the Unit of Work commits them.
public class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : IEntity
{
    protected readonly AppDbContext DbContext = dbContext;

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
        => await DbContext.Set<T>().AddAsync(entity, cancellationToken);

    public void Delete(T entity, CancellationToken cancellationToken)
        => DbContext.Set<T>().Remove(entity);

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken)
        => DbContext.Set<T>().AnyAsync(x => x.Id == id, cancellationToken);
}
