using Shop.Domain.Entities;

namespace Shop.Application.Abstract.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithCategoryAsync(CancellationToken ct = default);
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default);
    Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken ct = default);
}
