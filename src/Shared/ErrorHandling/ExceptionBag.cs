namespace Example.Shared.ErrorHandling;
public class ExceptionBag(ICollection<ApplicationException>? exceptions = null)
{
    public List<ApplicationException> Exceptions { get; } = exceptions?.ToList() ?? [];

    public void AddIfNotNull(ApplicationException? ex)
    {
        if (ex is not null) Exceptions.Add(ex);
    }

    public void ThrowIfAny()
    {
        if (Exceptions.Count == 1)
        {
            throw Exceptions[0];
        }

        if (Exceptions.Count > 0)
        {
            throw new AggregateException(Exceptions);
        }
    }
}
