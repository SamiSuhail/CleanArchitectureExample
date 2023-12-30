namespace Example.Shared.ErrorHandling.Clauses;

public static class ConflictExtensions
{
    public static ConflictException? Conflict<TValue>(
        this IClause? clause,
        string entityName,
        string keyName,
        TValue keyValue,
        bool hasConflict)
        where TValue : notnull
    {
        if (hasConflict is false)
        {
            return null;
        }

        var exception = ConflictException.New(entityName, keyName, keyValue);

        return clause is IGuardClause 
            ? throw exception
            : exception;
    }
}

public class ConflictException
    : ApplicationException
{
    public new const string DefaultCode = "CONFLICT";
    private const string MessageTemplateInternal = "{0} with {1} {2} was not found.";

    private ConflictException(string entityName, string keyName, string keyValue)
        : base(string.Format(MessageTemplateInternal, entityName, keyName, keyValue), DefaultCode)
    {
        EntityName = entityName;
        KeyName = keyName;
        KeyValue = keyValue;
    }

    public static string MessageTemplate { get; } = MessageTemplateInternal.Replace("0", nameof(EntityName))
                                                            .Replace("1", nameof(KeyName))
                                                            .Replace("2", nameof(KeyValue));

    public string EntityName { get; }
    public string KeyName { get; }
    public string KeyValue { get; }

    public static ConflictException New<TValue>(string entityName, string keyName, TValue keyValue)
        where TValue : notnull
        => new(entityName, keyName, keyValue?.ToString() ?? string.Empty);
}
