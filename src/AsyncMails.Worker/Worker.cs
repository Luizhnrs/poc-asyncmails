using AsyncMails.Application.Interfaces;
using AsyncMails.Domain.Enums;
using AsyncMails.Domain.Interfaces;

namespace AsyncMails.Worker;

/// <summary>
/// Background service responsible for processing async notification messages.
/// </summary>
public class NotificationWorker : BackgroundService
{
    private readonly INotificationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationWorker> _logger;

    public NotificationWorker(
        INotificationQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationWorker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var notification = await _queue.DequeueAsync(stoppingToken);

                _logger.LogInformation("Processing notification {Id}", notification.Id);

                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                // Atualiza status para Processing
                notification.Status = NotificationStatus.Processing;
                await repository.UpdateAsync(notification, stoppingToken);

                // Simula envio (delay)
                await Task.Delay(3000, stoppingToken); // 3 seconds delay

                // Atualiza status para Sent
                notification.Status = NotificationStatus.Sent;
                await repository.UpdateAsync(notification, stoppingToken);

                _logger.LogInformation("Notification {Id} sent successfully.", notification.Id);
            }
            catch (OperationCanceledException)
            {
                // Process is shutting down
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing notification worker.");
            }
        }

        _logger.LogInformation("NotificationWorker stopped at: {Time}", DateTimeOffset.Now);
    }
}
