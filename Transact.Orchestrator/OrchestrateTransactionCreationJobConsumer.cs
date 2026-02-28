using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;

namespace Transact.Orchestrator;

public class OrchestrateTransactionCreationJobConsumer(IConnection connection, IServiceProvider serviceProvider)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(OrchestratorMessaging.Queue, durable: true, exclusive: false, autoDelete: false,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            using var scope = serviceProvider.CreateScope();
            var orchestrateTransaction = scope.ServiceProvider.GetRequiredService<OrchestrateTransaction>();
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            var integrationEvent = JsonSerializer.Deserialize<IntegrationEvent>(message);
            if (integrationEvent is null)
            {
                return; 
            }

            await orchestrateTransaction.Orchestrate(integrationEvent); 
            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(OrchestratorMessaging.Queue, autoAck: false, consumer,
            cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }
}
