namespace Shop.Application.Models;

public record CategoryModel(int Id, string Name);

public record AddCategoryModel(string Name);

public record UpdateCategoryModel(int Id, string Name);

public record DeleteCategoryModel(int Id, int? MoveProductsTo);
