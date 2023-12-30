namespace Example.Shared.ErrorHandling.Clauses;

public static class NotFoundExtensions
{
    public static NotFoundException? NotFound<TEntity, TId>(
        this IClause? clause,
        string entityName,
        TEntity entity,
        TId id)
        where TId : notnull
    {
        if (entity is not null)
        {
            return null;
        }

        var exception = NotFoundException.New(entityName, id);

        return clause is IGuardClause 
            ? throw exception
            : exception;
    }
}

public class NotFoundException
    : ApplicationException
{
    public new const string DefaultCode = "NOT_FOUND";
    private const string MessageTemplateInternal = "{0} with key {1} was not found.";

    private NotFoundException(string entityName, string id)
        : base(string.Format(MessageTemplateInternal, entityName, id), DefaultCode)
    {
        EntityName = entityName;
        Id = id;
    }

    public static string MessageTemplate { get; } = MessageTemplateInternal.Replace("0", nameof(EntityName))
                                                            .Replace("1", nameof(Id));

    public string EntityName { get; }
    public string Id { get; }

    public static NotFoundException New<TId>(string entityName, TId id)
        where TId : notnull
        => new(entityName, id?.ToString() ?? string.Empty);
}
