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
using Transact.Orchestrator.Transaction;

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
            var repository = scope.ServiceProvider.GetRequiredService<OrchestrateRepository>();
            var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            var integrationEvent = JsonSerializer.Deserialize<IntegrationEvent>(message);
            if (integrationEvent is null)
            {
                return; 
            }
            
            switch (integrationEvent.EventType)
            {
                case ActionTypes.OrchestrateTransactionCreation:
                    await HandleTransactionTriggered(
                        integrationEvent,
                        message,
                        repository,
                        outboxService);
                    break;

                case ActionTypes.ReturnProductDetails:
                case ActionTypes.UserRequested:
                    await HandlePartialData(
                        integrationEvent,
                        repository,
                        outboxService);
                    break;
            }

            await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
        };

        await channel.BasicConsumeAsync(OrchestratorMessaging.Queue, autoAck: false, consumer,
            cancellationToken: stoppingToken);
        await Task.Delay(-1, stoppingToken);
    }

    private async Task HandlePartialData(IntegrationEvent integrationEvent, OrchestrateRepository repository, IOutboxService outboxService)
    {
        var currentStatus = await repository.GetStatusByCorrelationId(integrationEvent.CorrelationId);
        if( currentStatus.Status == "Triggered" )
        {
            currentStatus.Payload += integrationEvent.Payload;
            await repository.UpdateTransactionStatus(integrationEvent.CorrelationId, "PartiallyReady", integrationEvent.Payload);
        }
        if( currentStatus.Status == "PartiallyReady" )
        {
            var createTransactionIEvent = new CreateTransactionIntegrationEvent(integrationEvent.CorrelationId)
            {
                Products = GetProductsFromPayload(currentStatus.Payload),
                User = GetUserFromPayload(currentStatus.Payload),
            };
            await outboxService.SaveOutboxItemAsync(createTransactionIEvent,
                ActionTypes.CreateTransactionRequest); 
        }
    }
    
    private List<Product> GetProductsFromPayload(string payload)
    {
        return new List<Product>(); 
    }
    
    private User GetUserFromPayload(string payload)
    {
        return new User(); 
    }

    private async Task HandleTransactionTriggered(IntegrationEvent integrationEvent, string message, OrchestrateRepository repository, IOutboxService outboxService)
    {
        var orchestrateTransactionItem = new OrchestratorTransaction
        {
            CorrelationId = integrationEvent.CorrelationId,
            CreatedAt = DateTime.UtcNow,
            Status = "Triggered",
            EventToRaise = "TransactionCreationRequested",
            Payload = message
        };
        await repository.SaveOrchestratorTransaction(orchestrateTransactionItem);
        await outboxService.SaveOutboxItemAsync(integrationEvent, ActionTypes.GetProductDetails);
        await outboxService.SaveOutboxItemAsync(integrationEvent, ActionTypes.UserRequested); 
    }
}
