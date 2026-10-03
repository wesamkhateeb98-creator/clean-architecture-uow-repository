using Shop.Domain.Entities;

namespace Shop.Application.Abstract.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    // Throws BadRequestException when the category does not exist.
    Task EnsureExistsAsync(int id, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, CancellationToken ct = default);
}
