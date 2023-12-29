using Example.Application.Common.Interfaces;
using Example.Domain.Entities;
using Example.Domain.Enums;

namespace Example.Application.TodoItems.Commands.UpdateTodoItemDetail;

public record UpdateTodoItemDetailCommand : IRequest
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public PriorityLevel Priority { get; init; }

    public string? Note { get; init; }
}

public class UpdateTodoItemDetailCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<UpdateTodoItemDetailCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var todoItem = await dbContext.Repository<TodoItem>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(nameof(todoItem), todoItem, request.Id);

        todoItem!.ListId = request.ListId;
        todoItem.Priority = request.Priority;
        todoItem.Note = request.Note;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
