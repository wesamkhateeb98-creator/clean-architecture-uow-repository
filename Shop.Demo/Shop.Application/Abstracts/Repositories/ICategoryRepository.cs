using Shop.Domain.Entities;

namespace Shop.Application.Abstracts.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllAsync(CancellationToken cancellationToken);

    // Throws NotFoundException when the category does not exist.
    Task EnsureExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken);
}
