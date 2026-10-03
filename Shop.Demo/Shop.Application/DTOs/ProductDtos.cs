namespace Shop.Application.DTOs;

public record ProductDto(int Id, string Name, decimal Price, int Stock, int CategoryId, string CategoryName);

public record CreateProductRequest(string Name, decimal Price, int Stock, int CategoryId);

public record UpdateProductRequest(string Name, decimal Price, int Stock, int CategoryId);
