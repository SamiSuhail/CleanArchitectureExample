using Example.Application.Common.Interfaces;
using Example.Domain.Entities;

namespace Example.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var todoList = await dbContext.Repository<TodoList>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(nameof(todoList), todoList, request.Id);

        todoList!.Title = request.Title;

        await dbContext.SaveChangesAsync(cancellationToken);

    }
}
