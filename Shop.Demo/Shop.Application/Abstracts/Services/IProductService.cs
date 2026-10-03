using Shop.Application.Models;

namespace Shop.Application.Abstracts.Services;

public interface IProductService
{
    Task<List<ProductModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductModel> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(AddProductModel model, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
