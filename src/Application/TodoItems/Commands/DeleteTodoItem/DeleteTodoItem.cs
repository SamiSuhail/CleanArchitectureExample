using Example.Application.Common.Interfaces;
using Example.Domain.Entities;
using Example.Domain.Events;

namespace Example.Application.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(int Id) : IRequest;

public class DeleteTodoItemCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var todoItem = await dbContext.Repository<TodoItem>()
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(nameof(todoItem), todoItem, request.Id);

        dbContext.Repository<TodoItem>().Delete(todoItem!);

        todoItem!.AddDomainEvent(new TodoItemDeletedEvent(todoItem));

        await dbContext.SaveChangesAsync(cancellationToken);
    }

}
