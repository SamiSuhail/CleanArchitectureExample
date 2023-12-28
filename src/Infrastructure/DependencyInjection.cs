using Example.Application.Common.Interfaces;
using Example.Domain.Constants;
using Example.Infrastructure.Data;
using Example.Infrastructure.Data.Interceptors;
using Example.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    private record DatabaseOptions(
        int MaxRetryCount,
        int CommandTimeout,
        bool EnableDetailedErrors,
        bool EnableSensitiveDataLogging);

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var dbOptions = configuration
            .GetRequiredSection(nameof(DatabaseOptions))
            .Get<DatabaseOptions>()!;

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContextFactory<ApplicationDbContext>(
            (sp, options) =>
            {
                using (var scope = sp.CreateScope())
                    options.AddInterceptors(scope.ServiceProvider.GetServices<ISaveChangesInterceptor>());

                options.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(dbOptions.MaxRetryCount);
                    sqlServerOptions.CommandTimeout(dbOptions.CommandTimeout);
                });

                options.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(dbOptions.EnableSensitiveDataLogging);
            }, ServiceLifetime.Scoped);

        // Needed for initializer
        services.AddScoped(provider
            => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
                .CreateDbContext());

        services.AddScoped<IApplicationDbContextFactory, ApplicationDbContextFactory>();

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
