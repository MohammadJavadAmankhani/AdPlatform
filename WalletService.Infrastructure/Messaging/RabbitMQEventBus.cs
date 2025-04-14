using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WalletService.Core.Interfaces;

namespace WalletService.Infrastructure.Messaging
{
    public class RabbitMQEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQEventBus(string connectionString)
        {
            try
            {
                Console.WriteLine("----------1-----------");
                var factory = new ConnectionFactory { Uri = new Uri(connectionString) };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare("wallet-events", durable: true, exclusive: false, autoDelete: false);
                Console.WriteLine("RabbitMQ connected and wallet-events queue declared");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ connection error: {ex.Message}");
                throw;
            }
        }

        public async Task PublishAsync<T>(T @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
                _channel.BasicPublish(exchange: "", routingKey: "wallet-events", body: body);
                Console.WriteLine($"Published to wallet-events: {JsonSerializer.Serialize(@event)}");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error publishing to wallet-events: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                _channel?.Close();
                _connection?.Close();
                Console.WriteLine("RabbitMQ connection closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing RabbitMQ: {ex.Message}");
            }
        }
    }
}