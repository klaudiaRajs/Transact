using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Transact.Core.Contracts;

namespace Infrastructure;

public class TransactionRabbitMqInitializer(IConnection connection) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        await using var channel = await connection.CreateChannelAsync(cancellationToken: ct);
        await RunBinding(channel, ct, TransactionMessaging.Exchange, TransactionMessaging.Queue, TransactionMessaging.RoutingKey);
        await RunBinding(channel, ct, OrchestratorMessaging.Exchange, OrchestratorMessaging.Queue, OrchestratorMessaging.RoutingKey);
        await RunBinding(channel, ct, ProductMessaging.Exchange, ProductMessaging.Queue, ProductMessaging.RoutingKey);
        await RunBinding(channel, ct, UserMessaging.Exchange, UserMessaging.Queue, UserMessaging.RoutingKey);
        
        Console.WriteLine($"Connection open? {connection.IsOpen}");
    }

    private async Task RunBinding(IChannel channel, CancellationToken ct, string exchange, string queue, string routingKey)
    {
        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true, cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false, cancellationToken: ct);

        await channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: routingKey, cancellationToken: ct);
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
