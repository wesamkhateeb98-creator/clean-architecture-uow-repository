using Microsoft.EntityFrameworkCore;
using Shop.Application.Abstract.Repositories;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

// Generic implementation shared by all repositories.
// It only stages changes in the DbContext -- the Unit of Work commits them.
public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected readonly AppDbContext Context = context;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await Context.Set<T>().FindAsync([id], ct);

    public Task<List<T>> GetAllAsync(CancellationToken ct = default) =>
        Context.Set<T>().AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default) =>
        await Context.Set<T>().AddAsync(entity, ct);

    public void Remove(T entity) =>
        Context.Set<T>().Remove(entity);
}
