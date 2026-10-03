namespace Shop.Domain.Entities;

public class Product : IEntity
{
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public int CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private Product() { }

    public static Product Create(string name, decimal price, int stock, int categoryId) => new()
    {
        Name = name,
        Price = price,
        Stock = stock,
        CategoryId = categoryId
    };

    public void Update(string name, decimal price, int stock, int categoryId)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
    }

    public void MoveTo(int categoryId) => CategoryId = categoryId;
}
