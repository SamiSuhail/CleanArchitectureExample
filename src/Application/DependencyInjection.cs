using System.Reflection;
using Example.Application.Common.Behaviours;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var validatorsToRegister = AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
            .Where(pair => !ValidatorsHelper.IgnoredValidators.Contains(pair.ValidatorType))
            .Select(pair => ServiceDescriptor.Transient(pair.InterfaceType, pair.ValidatorType));

        foreach (var validator in validatorsToRegister)
        {
            services.Add(validator);
        };

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        return services;
    }
}
