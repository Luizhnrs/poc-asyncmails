using AsyncMails.Application.DTOs;
using AsyncMails.Domain.Entities;

namespace AsyncMails.Application.Services;

public interface INotificationService
{
    Task<Notification> ProcessCreateNotificationAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default);
}
