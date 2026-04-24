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
        services.AddScoped<AsyncMails.Application.Services.INotificationService, AsyncMails.Application.Services.NotificationService>();

        return services;
    }
}
