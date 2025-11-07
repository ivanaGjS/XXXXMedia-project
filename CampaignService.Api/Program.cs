using XXXXMedia.Shared.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Azure.Messaging.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Shared dependencies (DB)
builder.Services.AddSharedDependencies(builder.Configuration);

// Service Bus
builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
    return new ServiceBusClient(connectionString);
});

// Custom services
builder.Services.AddScoped<CampaignService.Api.Services.CampaignService>();

var app = builder.Build();


app.UseHttpsRedirection();
app.MapControllers();

app.Run();