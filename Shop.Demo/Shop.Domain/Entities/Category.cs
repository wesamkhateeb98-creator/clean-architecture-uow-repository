namespace Shop.Domain.Entities;

public class Category : IEntity
{
    public string Name { get; private set; } = string.Empty;

    public List<Product> Products { get; private set; } = [];

    private Category() { }

    public static Category Create(string name) => new() { Name = name };

    public void Rename(string name) => Name = name;
}
