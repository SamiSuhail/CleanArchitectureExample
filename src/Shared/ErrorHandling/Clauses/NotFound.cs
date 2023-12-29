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
    public const string MessageTemplate = "{0} with key {1} was not found.";
    public new const string DefaultCode = "NOT_FOUND";

    private NotFoundException(string entityName, string id)
        : base(string.Format(MessageTemplate, entityName, id), DefaultCode)
    {
        EntityName = entityName;
        Id = id;
    }

    public string EntityName { get; }
    public string Id { get; }

    public static NotFoundException New<TId>(string entityName, TId id)
        where TId : notnull
        => new(entityName, id?.ToString() ?? string.Empty);
}
