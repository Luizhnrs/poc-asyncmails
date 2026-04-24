namespace AsyncMails.Worker;

/// <summary>
/// Background service responsible for processing async notification messages.
/// Will consume messages from a queue (e.g., RabbitMQ, Azure Service Bus) in future iterations.
/// </summary>
public class NotificationWorker(ILogger<NotificationWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NotificationWorker started at: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("NotificationWorker polling at: {Time}", DateTimeOffset.Now);

            // Future: consume messages from a queue and process notifications
            await Task.Delay(5000, stoppingToken);
        }

        logger.LogInformation("NotificationWorker stopped at: {Time}", DateTimeOffset.Now);
    }
}
