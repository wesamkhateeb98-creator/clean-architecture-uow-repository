using Shop.Application.Abstract.Repositories;
using Shop.Infrastructure.Repositories;

namespace Shop.Infrastructure.Persistence;

// All repositories share the SAME DbContext instance,
// so one SaveChangesAsync commits every staged change in a single transaction.
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IProductRepository? _products;
    private ICategoryRepository? _categories;

    public IProductRepository Products => _products ??= new ProductRepository(context);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
