using AdManagementService.Application.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WalletService.Core.Domain.Events;

namespace AdManagementService.Infrastructure.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;

        public RabbitMQConsumer(string connectionString, IServiceProvider services)
        {
            var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("wallet-events", durable: true, exclusive: false, autoDelete: false);
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<WalletDeductedEvent>(message);
                Console.WriteLine($"Received WalletDeductedEvent: UserId={@event.UserId}, Amount={@event.Amount}");
                using var scope = _services.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<WalletDeductedEventHandler>();
                await handler.Handle(@event);
            };

            _channel.BasicConsume(queue: "wallet-events", autoAck: false, consumer: consumer);
            await Task.CompletedTask;
        }
    }
}
