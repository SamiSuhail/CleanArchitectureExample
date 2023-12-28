namespace Example.Domain.Entities.Common;

public interface ICreatableEntity : IEntity
{
    DateTime CreatedOn { get; set; }
    string? CreatedBy { get; set; }
}

public abstract class BaseCreatableEntity : BaseEntity, ICreatableEntity
{
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
}
