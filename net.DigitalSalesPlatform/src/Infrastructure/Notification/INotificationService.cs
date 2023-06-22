namespace Infrastructure.Notification;

public interface INotificationService
{
    void SendNotification<T>(T message) where T : class;
}