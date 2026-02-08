using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Transact.Core.Contracts;

namespace Infrastructure;

public class TransactionRabbitMqInitializer(IConnection connection) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        await using var channel = await connection.CreateChannelAsync(cancellationToken: ct);

        await channel.ExchangeDeclareAsync(
            exchange: TransactionMessaging.Exchange,
            type: ExchangeType.Direct,
            durable: true, cancellationToken: ct);

        await channel.QueueDeclareAsync(
            queue: TransactionMessaging.Queue,
            durable: true,
            exclusive: false,
            autoDelete: false, cancellationToken: ct);

        await channel.QueueBindAsync(
            queue: TransactionMessaging.Queue,
            exchange: TransactionMessaging.Exchange,
            routingKey: TransactionMessaging.RoutingKey, cancellationToken: ct);
        

        Console.WriteLine($"Connection open? {connection.IsOpen}");

    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
