using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Notification;

public class NotificationRabbitMqService : INotificationService
{
    public void SendNotification<T>(T message) where T : class
    {
        var factory = new ConnectionFactory
        {
            HostName = RabbitMqOptions.HostName,
            Port = RabbitMqOptions.Port
        };

        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();
        channel.QueueDeclare(RabbitMqOptions.QueueName, false, false, false, null);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish("", RabbitMqOptions.QueueName, null, body);
    }
}