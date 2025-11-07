using EventDrivenEmailSystem.Shared.DependencyInjection;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        services.AddEventDrivenShared(config);
        // register worker-specific services
        services.AddHostedService<EmailWorker>();
    })
    .Build();

await builder.RunAsync();

// Minimal placeholder worker implementation
public class EmailWorker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("EmailWorker running (placeholder). Replace with Service Bus subscription logic.");
        return Task.Delay(-1, stoppingToken);
    }
}
