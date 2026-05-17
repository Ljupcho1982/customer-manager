using blazor_project.Services.Interfaces;

namespace blazor_project.Services;

/// <summary>
/// Service implementation for notifications and toast messages.
/// </summary>
public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public event Action<string, string>? OnNotification;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger;
    }

    public void ShowSuccess(string message)
    {
        _logger.LogInformation(message);
        OnNotification?.Invoke(message, "success");
    }

    public void ShowError(string message)
    {
        _logger.LogError(message);
        OnNotification?.Invoke(message, "error");
    }

    public void ShowWarning(string message)
    {
        _logger.LogWarning(message);
        OnNotification?.Invoke(message, "warning");
    }

    public void ShowInfo(string message)
    {
        _logger.LogInformation(message);
        OnNotification?.Invoke(message, "info");
    }
}
