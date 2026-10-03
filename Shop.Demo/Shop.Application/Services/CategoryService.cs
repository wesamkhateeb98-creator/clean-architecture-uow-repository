using Shop.Application.Abstracts;
using Shop.Application.Abstracts.Services;
using Shop.Application.Extensions;
using Shop.Application.Models;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;

namespace Shop.Application.Services;

public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
    public async Task<List<CategoryModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.Categories.GetAllAsync(cancellationToken);
        return categories.Select(c => c.ToModel()).ToList();
    }

    public async Task<CategoryModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Category {id} not found.");

        return category.ToModel();
    }

    public async Task<int> AddAsync(AddCategoryModel model, CancellationToken cancellationToken)
    {
        var nameExists = await unitOfWork.Categories.NameExistsAsync(model.Name, cancellationToken);

        if (nameExists)
            throw new AlreadyExistsException($"Category '{model.Name}' already exists.");

        var category = Category.Create(model.Name);

        await unitOfWork.Categories.AddAsync(category, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return category.Id;
    }

    public async Task UpdateAsync(UpdateCategoryModel model, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(model.Id, cancellationToken)
            ?? throw new NotFoundException($"Category {model.Id} not found.");

        category.Rename(model.Name);
        await unitOfWork.CompleteAsync(cancellationToken);
    }

    // The Unit of Work showcase: two repositories, two saves, ONE transaction.
    // If deleting the category fails, moving the products is rolled back too.
    public async Task DeleteAsync(DeleteCategoryModel model, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(model.Id, cancellationToken)
            ?? throw new NotFoundException($"Category {model.Id} not found.");

        var products = await unitOfWork.Products.GetByCategoryAsync(model.Id, cancellationToken);
        var hasProducts = products.Count > 0;

        if (hasProducts)
        {
            if (model.MoveProductsTo is null)
                throw new FailedPreconditionException(
                    $"Category {model.Id} has {products.Count} products. Pass ?moveTo=<categoryId> to move them first.");

            if (model.MoveProductsTo == model.Id)
                throw new FailedPreconditionException("Cannot move products to the category being deleted.");

            await unitOfWork.Categories.EnsureExistsAsync(model.MoveProductsTo.Value, cancellationToken);
        }

        await unitOfWork.Transaction(async () =>
        {
            foreach (var product in products)
                product.MoveTo(model.MoveProductsTo!.Value);       // change #1 (Products repository)
            await unitOfWork.CompleteAsync(cancellationToken);

            unitOfWork.Categories.Delete(category, cancellationToken);  // change #2 (Categories repository)
            await unitOfWork.CompleteAsync(cancellationToken);
        }, cancellationToken);
    }
}
