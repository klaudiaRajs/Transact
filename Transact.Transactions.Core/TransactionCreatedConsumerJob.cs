using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Product;
using Transact.Core.Contracts.Transaction;
using Transact.Core.Contracts.User;
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
            var result = JsonSerializer.Deserialize<TransactionItem>(message);

            if (result != null)
            {
                using var scope = serviceProvider.CreateScope();
                var factory = scope.ServiceProvider.GetRequiredService<ITransactionFactory>();
                var user = JsonSerializer.Deserialize<User>(result.User.Payload);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var products = result.Products
                    .Select(pw =>
                    {
                        var prod = JsonSerializer.Deserialize<Product>(pw.Product.Payload, options)
                                   ?? throw new InvalidOperationException(
                                       $"Nie można zdeserializować produktu o correlationId={pw.Product.CorrelationId}");
                        return prod;
                    })
                    .ToList();
                var integrationEvent = new CreateTransactionIntegrationEvent()
                {
                    CorrelationId = result.User.CorrelationId,
                    User = user,
                    Products = products
                }; 
                factory.CreateTransaction(integrationEvent);
                Console.WriteLine($"Processed message: {integrationEvent.EventType}");
                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(TransactionMessaging.Queue, autoAck: false, consumer, cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }
}

