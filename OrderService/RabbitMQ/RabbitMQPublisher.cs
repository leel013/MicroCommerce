using RabbitMQ.Client;
using SharedContract.Model;
using System.Text;
using System.Text.Json;

namespace OrderService.RabbitMQ
{
    public class RabbitMQPublisher
    {
        public async Task PublishOrderCreated(OrderCreatedEvent order)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            await using var connection = await factory.CreateConnectionAsync();

            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: "OrderExchange",
                type: ExchangeType.Fanout,
                durable: true);

            var json = JsonSerializer.Serialize(order);

            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "OrderExchange",
                routingKey: "",
                body: body);
        }
    }
}
