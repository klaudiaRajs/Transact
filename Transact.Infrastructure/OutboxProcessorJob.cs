using System.Text.Json;
using Infrastructure.IntegrationEvents;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using Transact.Core.Contracts;

namespace Infrastructure;

[DisallowConcurrentExecution]
public class OutboxProcessorJob (IOutboxRepository outboxRepository, RabbitMqEventBus dispatcher, ILogger<OutboxProcessorJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = outboxRepository.GetAllUnprocessedMessages();
       // Console.WriteLine(messages.Count());
        foreach (var message in messages)
        {
            //Azure + try catch - no no 
            //refactor 
            try
            {   
                message.Type = "CreateTransactionRequest"; // Temporary fix for type resolution
                var type = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == message.Type);
                var deserializedMessage = JsonSerializer.Deserialize(message.Payload, type, JsonSerializerOptions.Default);
                var item = new CreateTransactionIntegrationEvent(message.CorrelationId); 
                item.CreateTransactionRequest = deserializedMessage as CreateTransactionRequest;
                
                //Zamień to na correlationId takie jak ma być
                var integrationEvent = new IntegrationEvent(Guid.NewGuid())
                {
                    EventType = message.Type!,
                    RoutingKey = TransactionMessaging.RoutingKey!, 
                    OccurredAt = DateTime.UtcNow,
                    Payload = message.Payload
                };
                
                await dispatcher.PublishAsync(
                    integrationEvent,
                    context.CancellationToken);
                
                await outboxRepository.UpdateProcessedOnAsync(message.Id, DateTime.UtcNow);
                logger.LogInformation($"Processed message and updated it's state for: {message.CorrelationId}, service: {nameof(Infrastructure)}");
            } catch (Exception ex)
            {
                logger.LogError($"Job process error: {message.CorrelationId}, service: {nameof(Infrastructure)}, message: {ex.Message}");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
