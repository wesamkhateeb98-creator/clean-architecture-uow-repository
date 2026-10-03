using Shop.Domain.Entities;

namespace Shop.Application.Abstracts;

// Generic repository: the operations every entity shares.
// Note: there is no Save here -- committing belongs to the Unit of Work.
public interface IRepository<T> where T : IEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Delete(T entity, CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
}
