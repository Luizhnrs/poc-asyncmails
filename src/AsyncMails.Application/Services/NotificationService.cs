using AsyncMails.Application.DTOs;
using AsyncMails.Application.Interfaces;
using AsyncMails.Domain.Entities;
using AsyncMails.Domain.Interfaces;

namespace AsyncMails.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationQueue _notificationQueue;

    public NotificationService(
        INotificationRepository notificationRepository,
        INotificationQueue notificationQueue)
    {
        _notificationRepository = notificationRepository;
        _notificationQueue = notificationQueue;
    }

    public async Task<Notification> ProcessCreateNotificationAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default)
    {
        // Check idempotency
        if (!string.IsNullOrWhiteSpace(request.IdempotencyKey))
        {
            var existingNotification = await _notificationRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey, cancellationToken);
            if (existingNotification != null)
            {
                return existingNotification;
            }
        }

        // Create new notification
        var notification = new Notification
        {
            UserId = request.UserId,
            Message = request.Message,
            Type = request.Type,
            IdempotencyKey = request.IdempotencyKey
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);

        // Send to background queue for processing
        await _notificationQueue.EnqueueAsync(notification, cancellationToken);

        return notification;
    }
}
