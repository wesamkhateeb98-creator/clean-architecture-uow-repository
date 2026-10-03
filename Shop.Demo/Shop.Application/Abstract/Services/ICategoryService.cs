using Shop.Application.DTOs;

namespace Shop.Application.Abstract.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);
    Task<CategoryDto> GetByIdAsync(int id, CancellationToken ct);
    Task<int> CreateAsync(CreateCategoryRequest request, CancellationToken ct);
    Task UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken ct);

    // Moves the category's products to moveProductsTo, then deletes it, in one transaction.
    Task DeleteAsync(int id, int? moveProductsTo, CancellationToken ct);
}
