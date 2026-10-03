using Shop.Application.Models;
using Shop.Domain.Entities;

namespace Shop.Application.Extensions;

public static class MapperExtensions
{
    public static ProductModel ToModel(this Product product) =>
        new(product.Id, product.Name, product.Price, product.Stock, product.CategoryId, product.Category?.Name ?? string.Empty);

    public static CategoryModel ToModel(this Category category) =>
        new(category.Id, category.Name);
}
