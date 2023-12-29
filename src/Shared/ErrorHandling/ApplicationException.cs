namespace Example.Shared.ErrorHandling;

public abstract class ApplicationException(string? message, string? code)
    : Exception(message)
{
    public const string DefaultCode = "UNEXPECTED";

    public ApplicationException(string? message)
        : this(message, null)
    { }

    public string Code { get; } = code ?? DefaultCode;
}
