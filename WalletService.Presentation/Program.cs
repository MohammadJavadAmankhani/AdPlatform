using Microsoft.EntityFrameworkCore;
using WalletService.Core.Interfaces;
using WalletService.Infrastructure.Data;
using WalletService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WalletService.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWalletCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeductWalletCommand).Assembly));

builder.Services.AddControllers();

builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddScoped<IEventBus>(sp =>
    new RabbitMQEventBus(builder.Configuration.GetConnectionString("RabbitMQ") ?? "amqp://guest:guest@localhost:5672/"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure middleware
app.UseAuthorization();
app.MapControllers();

app.Run();