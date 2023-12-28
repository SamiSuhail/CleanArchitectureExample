namespace Example.Domain.Entities.Common;

public interface IAuditableEntity : ICreatableEntity
{
    DateTime LastModifiedOn { get; set; }
    string? LastModifiedBy { get; set; }
}

public abstract class BaseAuditableEntity : BaseCreatableEntity, IAuditableEntity
{
    public DateTime LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
