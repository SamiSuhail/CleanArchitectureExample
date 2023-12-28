using Example.Domain.Entities.Common;

namespace Example.Application.Common.Interfaces;

public interface IApplicationDbContext : IAsyncDisposable
{
    /// <inheritdoc cref="DbSet{TEntity}" />
    IRepository<TEntity> Repository<TEntity>()
        where TEntity : class;

    /// <seealso cref="Repository{TEntity}"/>
    /// <returns>The <see cref="DbSet{TEntity}"/> as a <see cref="IDeletableRepository{TEntity}"/></returns>
    IDeletableRepository<TEntity> DeletableRepository<TEntity>()
        where TEntity : class, IDeletableEntity;

    /// <inheritdoc cref="DbContext.SaveChanges()" />
    int SaveChanges();

    /// <inheritdoc cref="DbContext.SaveChanges(bool)" />
    int SaveChanges(bool acceptAllChangesOnSuccess);

    /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)" />
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <inheritdoc cref="DbContext.SaveChangesAsync(bool, CancellationToken)" />
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
}

public interface IApplicationDbContextFactory
{
    IApplicationDbContext CreateDbContext();
    Task<IApplicationDbContext> CreateDbContextAsync(CancellationToken cancellationToken);
}
