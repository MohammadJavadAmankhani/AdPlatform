using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WalletService.Core.Interfaces;

namespace AdManagementService.Infrastructure.Messaging
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQEventBus(string connectionString)
        {
            try
            {
                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: "ad-events", durable: true, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine("RabbitMQEventBus: ad-events queue declared");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQEventBus: Connection error: {ex.Message}");
                throw;
            }
        }

        public async Task PublishAsync<T>(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
                _channel.BasicPublish(exchange: "", routingKey: "ad-events", basicProperties: null, body: body);
                Console.WriteLine($"Published to ad-events: {JsonSerializer.Serialize(@event)}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQEventBus: Error publishing: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();
                Console.WriteLine("RabbitMQEventBus: Connection closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQEventBus: Error closing: {ex.Message}");
            }
        }
    }
}