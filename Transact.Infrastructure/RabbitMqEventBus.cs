using System.Text;
using Infrastructure.IntegrationEvents;
using RabbitMQ.Client;

namespace Infrastructure;

public class RabbitMqEventBus(IConnection connection)
{
    public async Task PublishAsync(IntegrationEvent @event, CancellationToken ct)
    {
        try
        {
            await using var channel = await connection.CreateChannelAsync(cancellationToken: ct); 
            await channel.ExchangeDeclareAsync(@event.Exchange, ExchangeType.Direct, durable: true, cancellationToken: ct);
            await channel.BasicPublishAsync(exchange: @event.Exchange,
                routingKey: @event.RoutingKey,
                body: Encoding.UTF8.GetBytes(@event.Payload),
                ct);
        } catch (Exception ex)
        {
            Console.WriteLine($"Error publishing event: {ex.Message}");
        }

    }
}
