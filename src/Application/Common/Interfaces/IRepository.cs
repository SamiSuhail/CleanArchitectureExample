using Example.Domain.Entities.Common;

namespace Example.Application.Common.Interfaces;

public interface IRepository { }

/// <inheritdoc cref="DbSet{TEntity}" />
public interface IRepository<TEntity>
    : IRepository
    where TEntity : class
{
    /// <inheritdoc cref="DbSet{TEntity}.FindAsync(object?[]?, CancellationToken)" />
    ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken);

    /// <summary>
    /// Used to access the list of records in the repository
    /// </summary>
    /// <returns>The underlying DbSet as an IQueryable</returns>
    IQueryable<TEntity> Set();

    /// <inheritdoc cref="DbSet{TEntity}.AddAsync(TEntity, CancellationToken)" />
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <inheritdoc cref="DbSet{TEntity}.AddRangeAsync(IEnumerable{TEntity}, CancellationToken)" />
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    /// <inheritdoc cref="DbSet{TEntity}.Update(TEntity)" />
    void Update(TEntity entity);

    /// <inheritdoc cref="DbSet{TEntity}.UpdateRange(IEnumerable{TEntity})" />
    void UpdateRange(IEnumerable<TEntity> entities);

    /// <inheritdoc cref="DbSet{TEntity}.Remove(TEntity)" />
    void Delete(TEntity entity);

    /// <inheritdoc cref="DbSet{TEntity}.RemoveRange(IEnumerable{TEntity})" />
    void DeleteRange(IEnumerable<TEntity> entities);
}

public interface IDeletableRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
{
    /// <summary>
    /// Used to access the list of records in the repository, including deleted ones
    /// </summary>
    /// <returns>The unfiltered underlying DbSet as an IQueryable</returns>
    /// <seealso cref="IRepository{TEntity}.Set()" />
    IQueryable<TEntity> AllWithDeleted();

    /// <summary>
    /// Purges deletable entity from the database.
    /// </summary>
    /// <seealso cref="IRepository{TEntity}.Delete(TEntity)" />
    void HardDelete(TEntity entity);

    /// <summary>
    /// Purges deletable entities from the database.
    /// </summary>
    /// <seealso cref="IRepository{TEntity}.DeleteRange(TEntity[])" />
    void HardDeleteRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Sets <see cref="TEntity.IsDeleted"/> to <see cref="false"/> on an entity.
    /// </summary>
    /// <param name="entity">The entity to undelete</param>
    void Undelete(TEntity entity);
}
