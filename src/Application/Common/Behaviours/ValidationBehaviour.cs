using ValidationException = Example.Application.Common.Exceptions.ValidationException;

namespace Example.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validatorsToApply = _validators.Where(v =>
        {
            foreach (var ignoredValidatorType in ValidatorsHelper.IgnoredValidators)
            {
                if (v.GetType() == ignoredValidatorType)
                {
                    return false;
                }
            }

            return true;
        });

        if (validatorsToApply.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validatorsToApply.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }
        return await next();
    }
}

public static class ValidatorsHelper
{
    public static readonly HashSet<Type> IgnoredValidators = [];
}
