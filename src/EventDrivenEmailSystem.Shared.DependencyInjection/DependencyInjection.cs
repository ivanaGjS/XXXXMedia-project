using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Azure.Messaging.ServiceBus;

namespace EventDrivenEmailSystem.Shared.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEventDrivenShared(this IServiceCollection services, IConfiguration config)
    {
        services.AddMessaging(config);
        services.AddEmailing(config);
        services.AddPersistence(config);
        services.AddLogging();
        services.AddMemoryCache();
        return services;
    }

    private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration config)
    {
        var sbConn = config["AzureServiceBus:ConnectionString"];
        if (!string.IsNullOrEmpty(sbConn))
        {
            services.AddSingleton(new ServiceBusClient(sbConn));
        }
        // Register adapters (placeholders)
        services.AddScoped<IEventPublisher, AzureServiceBusPublisher>();
        services.AddScoped<IEventSubscriber, AzureServiceBusSubscriber>();
        return services;
    }

    private static IServiceCollection AddEmailing(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEmailSender, SendGridEmailSender>();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        // Shared persistence registrations (if any)
        return services;
    }
}

// Placeholder interfaces and classes (simple implementations)
// In real projects move these to shared contracts and proper namespaces.

public interface IEventPublisher { Task PublishAsync<T>(T @event) where T : class; }
public interface IEventSubscriber { Task SubscribeAsync(string topic, Func<string, Task> handler); }
public interface IEmailSender { Task SendAsync(string to, string subject, string body); }

public class AzureServiceBusPublisher : IEventPublisher
{
    private readonly ServiceBusClient _client;
    public AzureServiceBusPublisher(ServiceBusClient client) => _client = client;
    public async Task PublishAsync<T>(T @event) where T : class
    {
        var sender = _client.CreateSender("campaigns");
        var json = System.Text.Json.JsonSerializer.Serialize(@event);
        await sender.SendMessageAsync(new ServiceBusMessage(json));
    }
}

public class AzureServiceBusSubscriber : IEventSubscriber
{
    public Task SubscribeAsync(string topic, Func<string, Task> handler) => Task.CompletedTask;
}

public class SendGridEmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string body) => Task.CompletedTask;
}
