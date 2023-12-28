namespace Example.Domain.Entities.Common;

public interface IDeletableEntity : IAuditableEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedOn { get; set; }
    string? DeletedBy { get; set; }
}

public abstract class BaseDeletableEntity : BaseAuditableEntity, IDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
}
