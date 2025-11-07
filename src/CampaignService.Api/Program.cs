using Microsoft.EntityFrameworkCore;
using EventDrivenEmailSystem.Shared.DependencyInjection;
using CampaignService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Shared infra (Service Bus, Email sender, Blob, etc.)
builder.Services.AddEventDrivenShared(config);

// EF Core DbContext for Campaign
builder.Services.AddDbContext<CampaignDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("CampaignDb")));

// Register application specific DI (example placeholders)
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<CreateCampaignCommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
