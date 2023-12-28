using Example.Domain.Entities.Common;

namespace Example.Domain.Entities;

public class TodoList : BaseAuditableEntity, IIntIdentifiableEntity
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public Colour Colour { get; set; } = Colour.White;

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
