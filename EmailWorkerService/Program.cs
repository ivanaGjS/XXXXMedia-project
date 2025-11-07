using Azure.Messaging.ServiceBus;
using EmailWorkerService;
using EmailWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using XXXXMedia.Shared.DependencyInjection;
using XXXXMedia.Shared.Persistence;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        // Add shared persistence
        services.AddDbContext<SharedDbContext>(options =>
            options.UseSqlServer(ctx.Configuration.GetConnectionString("DefaultConnection")));

        services.AddSharedDependencies(ctx.Configuration);

        // Add repositories via shared DI if available
        services.AddScoped<IEmailSender, SendGridEmailSender>();

        // Message Bus client
        var sbConn = ctx.Configuration.GetValue<string>("ServiceBus:ConnectionString");
        if (!string.IsNullOrEmpty(sbConn))
            services.AddSingleton(new ServiceBusClient(sbConn));

        services.AddHostedService<EmailWorker>();
    })
    .Build();

await host.RunAsync();