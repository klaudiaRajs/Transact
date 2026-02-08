using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;
public class TransactionCreatedConsumerJob(IConnection connection, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken); 
        await channel.QueueDeclareAsync(TransactionMessaging.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var integrationEvent = JsonSerializer.Deserialize<CreateTransactionRequest>(message);

            if (integrationEvent != null)
            {
                using var scope = serviceProvider.CreateScope();
                var factory = scope.ServiceProvider.GetRequiredService<ITransactionFactory>();
                factory.CreateTransaction(integrationEvent);
                Console.WriteLine($"Processed message: {message}");
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(TransactionMessaging.Queue, autoAck: false, consumer, cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }
}

