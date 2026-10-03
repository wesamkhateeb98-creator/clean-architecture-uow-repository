using Shop.Domain.Entities;

namespace Shop.Application.Abstracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllWithCategoryAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken cancellationToken);
    Task<List<Product>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken);
}
