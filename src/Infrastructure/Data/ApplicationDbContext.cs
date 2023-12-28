using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using Example.Application.Common.Interfaces;
using Example.Domain.Entities.Common;
using Example.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Example.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    private const string DebugRepositoryMessage = $"You should use {nameof(DeletableRepository)} instead.";

    private readonly ConcurrentDictionary<Type, IRepository> _existingRepositories = [];

    public IRepository<TEntity> Repository<TEntity>()
        where TEntity : class
    {
        Debug.Assert(typeof(TEntity) is not IDeletableEntity, DebugRepositoryMessage);

        return (IRepository<TEntity>) _existingRepositories.GetOrAdd(
            typeof(TEntity), 
            (_) => new Repository<TEntity>(this));
    }

    public IDeletableRepository<TEntity> DeletableRepository<TEntity>()
        where TEntity : class, IDeletableEntity
        => (IDeletableRepository<TEntity>) _existingRepositories.GetOrAdd(
            typeof(TEntity), 
            (_) => new DeletableRepository<TEntity>(this));

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
public class ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> dbFactory)
    : IApplicationDbContextFactory
{
    public IApplicationDbContext CreateDbContext() => dbFactory.CreateDbContext();
    public async Task<IApplicationDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
        => await dbFactory.CreateDbContextAsync(cancellationToken);
}
