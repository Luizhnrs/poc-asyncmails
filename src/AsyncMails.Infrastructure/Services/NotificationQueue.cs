using System.Threading.Channels;
using AsyncMails.Application.Interfaces;
using AsyncMails.Domain.Entities;

namespace AsyncMails.Infrastructure.Services;

public class NotificationQueue : INotificationQueue
{
    private readonly Channel<Notification> _channel;

    public NotificationQueue()
    {
        var options = new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<Notification>(options);
    }

    public async ValueTask EnqueueAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(notification);
        await _channel.Writer.WriteAsync(notification, cancellationToken);
    }

    public async ValueTask<Notification> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}
