using Microsoft.EntityFrameworkCore;
using AdManagementService.Core.Interfaces;
using AdManagementService.Infrastructure.Data;
using AdManagementService.Infrastructure.Cache;
using AdManagementService.Infrastructure.Clients;
using AdManagementService.Infrastructure.Messaging;
using StackExchange.Redis;
using MediatR;
using AdManagementService.Application.Commands;
using WalletService.Core.Interfaces;
using AdManagementService.Application.Events;
using WalletService.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAdCommand).Assembly));


builder.Services.AddDbContext<AdDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));

builder.Services.AddHostedService(sp =>
    new RabbitMQConsumer(builder.Configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672/", sp));

builder.Services.AddScoped<IEventBus>(sp =>
    new AdManagementService.Infrastructure.Messaging.RabbitMQEventBus(builder.Configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672/"));

builder.Services.AddScoped<IAdRepository, AdRepository>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddScoped<WalletDeductedEventHandler>();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddHttpClient<IWalletClient, WalletClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
});

var app = builder.Build();

// Configure middleware
app.UseAuthorization();
app.MapControllers();

app.Run();