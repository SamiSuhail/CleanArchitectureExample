using Example.Application.Common.Interfaces;
using Example.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Data;

internal class DeletableRepository<TEntity>(ApplicationDbContext context)
    : Repository<TEntity>(context), IDeletableRepository<TEntity>
        where TEntity : class, IDeletableEntity
{
    public override IQueryable<TEntity> Set()
        => base.Set().Where(x => !x.IsDeleted);
    public IQueryable<TEntity> AllWithDeleted()
        => base.Set().IgnoreQueryFilters();

    public void HardDelete(TEntity entity)
        => base.Delete(entity);

    public void HardDeleteRange(IEnumerable<TEntity> entities)
        => base.DeleteRange(entities);

    public void Undelete(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedOn = null;
        Update(entity);
    }

    public override void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
        Update(entity);
    }
}
