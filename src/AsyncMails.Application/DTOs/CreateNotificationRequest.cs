namespace AsyncMails.Application.DTOs;

public class CreateNotificationRequest
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string IdempotencyKey { get; set; } = string.Empty;
}
