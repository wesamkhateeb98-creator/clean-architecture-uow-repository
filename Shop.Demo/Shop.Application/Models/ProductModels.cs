namespace Shop.Application.Models;

public record ProductModel(int Id, string Name, decimal Price, int Stock, int CategoryId, string CategoryName);

public record AddProductModel(string Name, decimal Price, int Stock, int CategoryId);

public record UpdateProductModel(int Id, string Name, decimal Price, int Stock, int CategoryId);
