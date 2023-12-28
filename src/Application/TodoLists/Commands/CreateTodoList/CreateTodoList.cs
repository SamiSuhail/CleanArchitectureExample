using Example.Application.Common.Interfaces;
using Example.Domain.Entities;

namespace Example.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler(IApplicationDbContextFactory dbContextFactory)
    : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContextFactory _dbContextFactory = dbContextFactory;

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var entity = new TodoList
        {
            Title = request.Title,
        };

        await dbContext.Repository<TodoList>()
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
