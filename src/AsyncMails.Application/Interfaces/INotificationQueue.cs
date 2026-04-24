using AsyncMails.Domain.Entities;

namespace AsyncMails.Application.Interfaces;

public interface INotificationQueue
{
    ValueTask EnqueueAsync(Notification notification, CancellationToken cancellationToken = default);
    ValueTask<Notification> DequeueAsync(CancellationToken cancellationToken = default);
}
