namespace blazor_project.Services.Interfaces;

/// <summary>
/// Service interface for notifications and toasts.
/// </summary>
public interface INotificationService
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowWarning(string message);
    void ShowInfo(string message);
}
