using System.Text;
using System.Text.Json;
using Infrastructure.IntegrationEvents;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transact.Core.Contracts;
using Transact.Core.Contracts.Infrastructure;

namespace Transact.Core.Products;

public class ProductDetailsJobConsumer(IConnection connection, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
          var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken); 
        await channel.QueueDeclareAsync(ProductMessaging.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var integrationEvent = JsonSerializer.Deserialize<CreateTransactionRequest>(message);

            if (integrationEvent != null)
            {
                using var scope = serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<ProductFactory>();
                var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
                var productDetails = repository.GetProductsByIds(integrationEvent.ProductIds.Split(',').Select(int.Parse));
                var productDetailsEvent = new ProductDetailsIntegrationEvent("integrationEvent.CorrelationId")
                {
                    Payload = JsonSerializer.Serialize(productDetails),
                };
                await outboxService.SaveOutboxItemAsync(productDetailsEvent, ActionTypes.ReturnProductDetails); 
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(ProductMessaging.Queue, autoAck: false, consumer, cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }
}
