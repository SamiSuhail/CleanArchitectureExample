using Example.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Data;

public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
{
    public Repository(ApplicationDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        Context = context;
        DbSet = Context.Set<TEntity>();
    }

    protected DbSet<TEntity> DbSet { get; set; }
    protected ApplicationDbContext Context { get; set; }

    public virtual ValueTask<TEntity?> FindAsync(object?[]? keyValues, CancellationToken cancellationToken)
        => DbSet.FindAsync(keyValues, cancellationToken);
    public virtual IQueryable<TEntity> Set() => DbSet;

    public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        => DbSet.AddAsync(entity, cancellationToken).AsTask();
    public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        => DbSet.AddRangeAsync(entities, cancellationToken);

    public virtual void Update(TEntity entity) => DbSet.Update(entity);
    public virtual void UpdateRange(IEnumerable<TEntity> entities) => DbSet.UpdateRange(entities);

    public virtual void Delete(TEntity entity) => DbSet.Remove(entity);
    public virtual void DeleteRange(IEnumerable<TEntity> entities) => DbSet.RemoveRange(entities);
}
