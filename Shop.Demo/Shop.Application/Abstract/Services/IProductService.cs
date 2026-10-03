using Shop.Application.DTOs;

namespace Shop.Application.Abstract.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(CancellationToken ct);
    Task<ProductDto> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(CreateProductRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}
