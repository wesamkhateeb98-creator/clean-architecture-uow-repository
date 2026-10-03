using Shop.Application.DTOs;
using Shop.Application.Exceptions;
using Shop.Application.Interfaces;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

// Depends only on IUnitOfWork (an interface) -- it has no idea EF Core or PostgreSQL exist.
public class ProductService(IUnitOfWork unitOfWork)
{
    public async Task<List<ProductDto>> GetAllAsync(CancellationToken ct)
    {
        var products = await unitOfWork.Products.GetAllWithCategoryAsync(ct);
        return products.Select(ToDto).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var product = await unitOfWork.Products.GetByIdWithCategoryAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.");

        return ToDto(product);
    }

    public async Task<int> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        await unitOfWork.Categories.EnsureExistsAsync(request.CategoryId, ct);

        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };

        await unitOfWork.Products.AddAsync(product, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return product.Id;
    }

    public async Task UpdateAsync(int id, UpdateProductRequest request, CancellationToken ct)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.");

        await unitOfWork.Categories.EnsureExistsAsync(request.CategoryId, ct);

        product.Name = request.Name;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var product = await unitOfWork.Products.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Product {id} not found.");

        unitOfWork.Products.Remove(product);
        await unitOfWork.SaveChangesAsync(ct);
    }

    private static ProductDto ToDto(Product p) =>
        new(p.Id, p.Name, p.Price, p.Stock, p.CategoryId, p.Category?.Name ?? string.Empty);
}
