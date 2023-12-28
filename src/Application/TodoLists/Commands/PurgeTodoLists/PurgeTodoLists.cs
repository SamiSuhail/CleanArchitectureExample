using Example.Application.Common.Interfaces;
using Example.Application.Common.Security;
using Example.Domain.Constants;
using Example.Domain.Entities;

namespace Example.Application.TodoLists.Commands.PurgeTodoLists;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanPurge)]
public record PurgeTodoListsCommand : IRequest;

public class PurgeTodoListsCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<PurgeTodoListsCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        dbContext.Repository<TodoList>().DeleteRange(dbContext.Repository<TodoList>().Set());

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
