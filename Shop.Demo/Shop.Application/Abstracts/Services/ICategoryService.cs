using Shop.Application.Models;

namespace Shop.Application.Abstracts.Services;

public interface ICategoryService
{
    Task<List<CategoryModel>> GetAllAsync(CancellationToken cancellationToken);
    Task<CategoryModel> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> AddAsync(AddCategoryModel model, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateCategoryModel model, CancellationToken cancellationToken);

    // Moves the category's products to MoveProductsTo, then deletes it -- in one transaction.
    Task DeleteAsync(DeleteCategoryModel model, CancellationToken cancellationToken);
}
