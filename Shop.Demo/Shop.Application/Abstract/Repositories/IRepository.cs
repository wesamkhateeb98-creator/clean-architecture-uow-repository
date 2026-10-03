namespace Shop.Application.Abstract.Repositories;

// Generic repository: the operations every entity shares.
// Note: there is no SaveChanges here -- saving belongs to the Unit of Work.
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    void Remove(T entity);
}
