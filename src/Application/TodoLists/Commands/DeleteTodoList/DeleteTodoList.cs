using Example.Application.Common.Interfaces;
using Example.Domain.Entities;

namespace Example.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(int Id) : IRequest;

public class DeleteTodoListCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var todoList = await dbContext.Repository<TodoList>()
            .Set()
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(nameof(todoList), todoList, request.Id);

        dbContext.Repository<TodoList>().Delete(todoList!);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
