using XXXXMedia.Shared.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSharedDependencies(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
