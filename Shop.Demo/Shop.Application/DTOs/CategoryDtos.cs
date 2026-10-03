namespace Shop.Application.DTOs;

public record CategoryDto(int Id, string Name);

public record CreateCategoryRequest(string Name);

public record UpdateCategoryRequest(string Name);
