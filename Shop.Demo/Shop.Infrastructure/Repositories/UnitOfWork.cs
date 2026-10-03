using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Shop.Application.Abstracts;
using Shop.Application.Abstracts.Repositories;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

// All repositories share the SAME DbContext instance,
// so CompleteAsync commits every staged change together.
public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private static readonly ResiliencePipeline _retryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<DbUpdateException>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromMilliseconds(200),
            BackoffType = DelayBackoffType.Exponential
        })
        .Build();

    private IProductRepository? _products;
    private ICategoryRepository? _categories;

    public IProductRepository Products => _products ??= new ProductRepository(dbContext);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(dbContext);

    public Task CompleteAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);

    // Several CompleteAsync calls inside doTransaction -> one BEGIN ... COMMIT (or ROLLBACK).
    public async Task Transaction(Func<Task> doTransaction, CancellationToken cancellationToken)
    {
        await _retryPipeline.ExecuteAsync(async ct =>
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync(ct);
            try
            {
                await doTransaction();
                await transaction.CommitAsync(ct);
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }, cancellationToken);
    }
}
