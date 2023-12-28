using Example.Application.Common.Interfaces;
using Example.Domain.Entities;
using Example.Domain.Events;

namespace Example.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        await dbContext.Repository<TodoItem>()
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
