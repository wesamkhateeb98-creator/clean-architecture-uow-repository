using Shop.Domain.Entities;

namespace Shop.Application.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, CancellationToken ct = default);
}
