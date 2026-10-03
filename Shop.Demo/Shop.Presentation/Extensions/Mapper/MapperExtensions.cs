using Shop.Application.Models;
using Shop.Presentation.Models.Requests;

namespace Shop.Presentation.Extensions.Mapper;

// HTTP request (Presentation) -> Application model.
public static class MapperExtensions
{
    public static AddProductModel ToModel(this AddProductRequest request) =>
        new(request.Name, request.Price, request.Stock, request.CategoryId);

    public static UpdateProductModel ToModel(this UpdateProductRequest request, int id) =>
        new(id, request.Name, request.Price, request.Stock, request.CategoryId);

    public static AddCategoryModel ToModel(this AddCategoryRequest request) =>
        new(request.Name);

    public static UpdateCategoryModel ToModel(this UpdateCategoryRequest request, int id) =>
        new(id, request.Name);
}
