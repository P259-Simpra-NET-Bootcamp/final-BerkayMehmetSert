using Newtonsoft.Json;

namespace Infrastructure.Notification;

public class NotificationConsoleService : INotificationService
{
    public void SendNotification<T>(T message) where T : class
    {
        var body = JsonConvert.SerializeObject(message);
        Console.WriteLine($"Notification: {body}");
    }
}