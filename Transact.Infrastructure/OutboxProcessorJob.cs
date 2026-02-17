using System.Text.Json;
using Infrastructure.IntegrationEvents;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using Transact.Core.Contracts;
using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure;

[DisallowConcurrentExecution]
public class OutboxProcessorJob(
    IOutboxRepository outboxRepository,
    RabbitMqEventBus dispatcher,
    ILogger<OutboxProcessorJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = outboxRepository.GetAllUnprocessedMessages();
        foreach (var message in messages)
        {
            //Azure + try catch - no no 
            //refactor 
            try
            {
                var integrationEvent = new IntegrationEvent(message.CorrelationId)
                {
                    EventType = message.Type!,
                    OccurredAt = DateTime.UtcNow,
                    Payload = message.Payload
                };

                integrationEvent = GetRoutingKeyAndExchange(integrationEvent, message.Type!);

                await dispatcher.PublishAsync(
                    integrationEvent,
                    context.CancellationToken);

                await outboxRepository.UpdateProcessedOnAsync(message.Id, DateTime.UtcNow);
                logger.LogInformation(
                    $"Processed message and updated it's state for: {message.CorrelationId}, service: {nameof(Infrastructure)}");
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"Job process error: {message.CorrelationId}, service: {nameof(Infrastructure)}, message: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
    }
    //TODO move to RabbitMqEventBus
    private static IntegrationEvent GetRoutingKeyAndExchange(IntegrationEvent integrationEvent, string messageType)
    {
        var routingKey = "";
        var exchange = "";
        switch (messageType)
        {
            case ActionTypes.CreateTransactionRequest:
                routingKey = TransactionMessaging.RoutingKey;
                exchange = TransactionMessaging.Exchange;
                break;
            case ActionTypes.OrchestrateTransactionCreation:
            case ActionTypes.ReturnProductDetails:
                routingKey = OrchestratorMessaging.RoutingKey;
                exchange = OrchestratorMessaging.Exchange;
                break;
            case ActionTypes.GetProductDetails:
                routingKey = ProductMessaging.RoutingKey;
                exchange = ProductMessaging.Exchange;
                break;
        }

        integrationEvent.RoutingKey = routingKey;
        integrationEvent.Exchange = exchange;
        return integrationEvent;
    }
}
