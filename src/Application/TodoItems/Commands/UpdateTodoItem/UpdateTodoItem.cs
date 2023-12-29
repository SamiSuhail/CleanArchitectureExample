using Example.Application.Common.Interfaces;
using Example.Domain.Entities;

namespace Example.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand : IRequest
{
    public int Id { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }
}

public class UpdateTodoItemCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<UpdateTodoItemCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var todoItem = await dbContext.Repository<TodoItem>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(nameof(todoItem), todoItem, request.Id);

        todoItem!.Title = request.Title;
        todoItem.Done = request.Done;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
