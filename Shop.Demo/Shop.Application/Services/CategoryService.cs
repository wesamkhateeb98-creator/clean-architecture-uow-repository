using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.Interfaces;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

public class CategoryService(IUnitOfWork unitOfWork)
{
    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        var categories = await unitOfWork.Categories.GetAllAsync(ct);
        return categories.Select(c => new CategoryDto(c.Id, c.Name)).ToList();
    }

    public async Task<CategoryDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Category {id} not found.");

        return new CategoryDto(category.Id, category.Name);
    }

    public async Task<int> CreateAsync(CreateCategoryRequest request, CancellationToken ct)
    {
        if (await unitOfWork.Categories.NameExistsAsync(request.Name, ct))
            throw new BadRequestException($"Category '{request.Name}' already exists.");

        var category = new Category { Name = request.Name };

        await unitOfWork.Categories.AddAsync(category, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return category.Id;
    }

    public async Task UpdateAsync(int id, UpdateCategoryRequest request, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Category {id} not found.");

        category.Name = request.Name;
        await unitOfWork.SaveChangesAsync(ct);
    }

    // The Unit of Work showcase: two repositories, one commit.
    // Products are moved to another category AND the category is deleted --
    // both changes are saved together, or neither is.
    public async Task DeleteAsync(int id, int? moveProductsTo, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Category {id} not found.");

        var products = await unitOfWork.Products.GetByCategoryAsync(id, ct);

        if (products.Count > 0)
        {
            if (moveProductsTo is null)
                throw new BadRequestException(
                    $"Category {id} has {products.Count} products. Pass ?moveTo=<categoryId> to move them first.");

            if (moveProductsTo == id)
                throw new BadRequestException("Cannot move products to the category being deleted.");

            await unitOfWork.Categories.EnsureExistsAsync(moveProductsTo.Value, ct);

            foreach (var product in products)
                product.CategoryId = moveProductsTo.Value;      // change #1 (Products repository)
        }

        unitOfWork.Categories.Remove(category);                 // change #2 (Categories repository)

        await unitOfWork.SaveChangesAsync(ct);                  // ONE transaction for both
    }
}
