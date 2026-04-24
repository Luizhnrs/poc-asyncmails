using Microsoft.Extensions.DependencyInjection;

namespace AsyncMails.Application;

/// <summary>
/// Extension methods for registering Application layer services in the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services, use cases, validators, etc.
        // Example: services.AddScoped<ISendNotificationUseCase, SendNotificationUseCase>();

        return services;
    }
}
