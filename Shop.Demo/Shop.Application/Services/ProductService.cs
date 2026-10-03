using Shop.Application.Abstracts;
using Shop.Application.Abstracts.Services;
using Shop.Application.Extensions;
using Shop.Application.Models;
using Shop.Domain.Entities;
using Shop.Domain.Exceptions;

namespace Shop.Application.Services;

// Depends only on IUnitOfWork (an interface) -- it has no idea EF Core or PostgreSQL exist.
public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    public async Task<List<ProductModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var products = await unitOfWork.Products.GetAllWithCategoryAsync(cancellationToken);
        return products.Select(p => p.ToModel()).ToList();
    }

    public async Task<ProductModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdWithCategoryAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Product {id} not found.");

        return product.ToModel();
    }

    public async Task<int> AddAsync(AddProductModel model, CancellationToken cancellationToken)
    {
        await unitOfWork.Categories.EnsureExistsAsync(model.CategoryId, cancellationToken);

        var product = Product.Create(model.Name, model.Price, model.Stock, model.CategoryId);

        await unitOfWork.Products.AddAsync(product, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return product.Id;
    }

    public async Task UpdateAsync(UpdateProductModel model, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdAsync(model.Id, cancellationToken)
            ?? throw new NotFoundException($"Product {model.Id} not found.");

        await unitOfWork.Categories.EnsureExistsAsync(model.CategoryId, cancellationToken);

        product.Update(model.Name, model.Price, model.Stock, model.CategoryId);

        await unitOfWork.CompleteAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Product {id} not found.");

        unitOfWork.Products.Delete(product, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
    }
}
