namespace Example.Domain.Entities.Common;
public interface IIdentifiableEntity<TKey>
{
    public TKey Id { get; set; }
}

public interface IGuidIdentifiableEntity : IIdentifiableEntity<Guid>
{
}

public interface IIntIdentifiableEntity : IIdentifiableEntity<int>
{
}
