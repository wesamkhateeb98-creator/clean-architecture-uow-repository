namespace Shop.Presentation.Models.Requests;

public record AddProductRequest(string Name, decimal Price, int Stock, int CategoryId);

public record UpdateProductRequest(string Name, decimal Price, int Stock, int CategoryId);
