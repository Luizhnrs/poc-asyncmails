using AsyncMails.Domain.Entities;
using AsyncMails.Domain.Interfaces;
using AsyncMails.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsyncMails.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AsyncMailsDbContext _dbContext;

    public NotificationRepository(AsyncMailsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications.AddAsync(notification, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _dbContext.Notifications.Update(notification);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
