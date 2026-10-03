using Shop.Application.Abstracts.Repositories;

namespace Shop.Application.Abstracts;

// Unit of Work: one entry point to every repository + one place to commit their changes.
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }

    Task CompleteAsync(CancellationToken cancellationToken);
    Task Transaction(Func<Task> doTransaction, CancellationToken cancellationToken);
}
