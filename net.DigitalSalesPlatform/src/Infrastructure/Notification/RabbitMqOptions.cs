namespace Infrastructure.Notification;

public static class RabbitMqOptions
{
    public const string HostName = "localhost";
    public const int Port = 5672;
    public const string QueueName = "notification-queue";
}