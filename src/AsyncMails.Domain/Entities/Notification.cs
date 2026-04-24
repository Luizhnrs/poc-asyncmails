using AsyncMails.Domain.Enums;

namespace AsyncMails.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Notification()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        Status = NotificationStatus.Pending;
    }
}
