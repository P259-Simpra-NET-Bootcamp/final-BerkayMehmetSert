using Infrastructure.Notification;
using Infrastructure.Notification.Model;
using Infrastructure.Payment;
using Infrastructure.Payment.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureExtensions(this IServiceCollection services)
    {
        services.AddTransient<NotificationRabbitMqService>();
        services.AddTransient<NotificationConsoleService>();
        services.AddTransient<EftFakePaymentService>();
        services.AddTransient<CreditCardFakePaymentService>();
        services.AddTransient<Func<NotificationType, INotificationService>>(serviceProvider => type =>
        {
            return (type switch
            {
                NotificationType.RabbitMq => serviceProvider.GetService<NotificationRabbitMqService>(),
                NotificationType.Console => serviceProvider.GetService<NotificationConsoleService>(),
                _ => serviceProvider.GetService<NotificationRabbitMqService>()
            })!;
        });
        services.AddTransient<Func<PaymentMethod, IFakePaymentService>>(serviceProvider => type =>
        {
            return (type switch
            {
                PaymentMethod.Eft => serviceProvider.GetService<EftFakePaymentService>(),
                PaymentMethod.CreditCard => serviceProvider.GetService<CreditCardFakePaymentService>(),
                _ => serviceProvider.GetService<CreditCardFakePaymentService>()
            })!;
        });
    }
}