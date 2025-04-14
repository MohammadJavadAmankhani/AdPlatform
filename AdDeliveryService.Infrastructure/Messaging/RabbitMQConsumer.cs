using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using AdManagementService.Core.Domain;
using AdManagementService.Core.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using AdDeliveryService.Application.Events;
using Microsoft.Extensions.Hosting;

namespace AdDeliveryService.Infrastructure.Messaging
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;

        public RabbitMQConsumer(string connectionString, IServiceProvider services)
        {
            _services = services;
            var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "ad-events", durable: true, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine("AdEventsConsumer: ad-events queue declared");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AdEventsConsumer: RabbitMQ connection error: {ex.Message}");
                throw;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var json = JsonSerializer.Deserialize<JsonElement>(message);

                    if (!json.TryGetProperty("Type", out var typeProp))
                    {
                        Console.WriteLine("Error: Message missing Type property");
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                        return;
                    }

                    var eventType = typeProp.GetString();
                    using var scope = _services.CreateScope();

                    if (eventType == nameof(AdCreatedEvent))
                    {
                        var @event = JsonSerializer.Deserialize<AdCreatedEvent>(message);
                        if (@event == null)
                        {
                            Console.WriteLine("Error: Failed to deserialize AdCreatedEvent");
                            _channel.BasicNack(ea.DeliveryTag, false, true);
                            return;
                        }
                        Console.WriteLine($"Received AdCreatedEvent: AdId={@event}");
                        var handler = scope.ServiceProvider.GetRequiredService<AdCreatedEventHandler>();
                        await handler.Handle(@event);
                    }
                    else if (eventType == nameof(AdPausedEvent))
                    {
                        var @event = JsonSerializer.Deserialize<AdPausedEvent>(message);
                        if (@event == null)
                        {
                            Console.WriteLine("Error: Failed to deserialize AdPausedEvent");
                            _channel.BasicNack(ea.DeliveryTag, false, true);
                            return;
                        }
                        Console.WriteLine($"Received AdPausedEvent: AdId={@event.AdId}");
                        var handler = scope.ServiceProvider.GetRequiredService<AdPausedEventHandler>();
                        await handler.Handle(@event);
                    }
                    else
                    {
                        Console.WriteLine($"Unknown event type: {eventType}");
                        _channel.BasicNack(ea.DeliveryTag, false, true);
                        return;
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing ad event: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: "ad-events", autoAck: false, consumer: consumer);
            Console.WriteLine("AdEventsConsumer: Started consuming ad-events");
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();
                Console.WriteLine("AdEventsConsumer: RabbitMQ connection closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AdEventsConsumer: Error closing RabbitMQ: {ex.Message}");
            }
            base.Dispose();
        }
    }
}
