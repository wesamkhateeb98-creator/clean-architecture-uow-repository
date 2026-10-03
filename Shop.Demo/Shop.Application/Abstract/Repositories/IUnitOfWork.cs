namespace Shop.Application.Abstract.Repositories;

// Unit of Work: one entry point to every repository + ONE commit for all their changes.
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
