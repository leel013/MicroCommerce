using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedContract.Model;
using System.Text;
using System.Text.Json;

namespace NotificationService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            var connection = await factory.CreateConnectionAsync();

            var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "OrderExchange",
                type: ExchangeType.Fanout,
                durable: true);

            await channel.QueueDeclareAsync(
                queue: "NotificationQueue",
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: "NotificationQueue",
                exchange: "OrderExchange",
                routingKey: "");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var order =
                    JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                Console.WriteLine();
                Console.WriteLine("=================================");
                Console.WriteLine("Order Created Event Received");
                Console.WriteLine($"Order Id : {order!.OrderId}");
                Console.WriteLine($"Product Id : {order.ProductId}");
                Console.WriteLine($"Quantity : {order.Quantity}");
                Console.WriteLine("Sending Email...");
                await Task.Delay(1000);
                Console.WriteLine("Email Sent Successfully");
                Console.WriteLine("=================================");
                Console.WriteLine();

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(
                queue: "NotificationQueue",
                autoAck: false,
                consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
