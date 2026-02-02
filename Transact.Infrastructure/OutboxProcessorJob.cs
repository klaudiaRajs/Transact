using System.Text.Json;
using Infrastructure.EventBus;
using Infrastructure.Interfaces;
using Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using Quartz;
using Transact.Core.Contracts;

namespace Infrastructure;

[DisallowConcurrentExecution]
public class OutboxProcessorJob (IOutboxRepository outboxRepository, IMessageDispatcher dispatcher, ILogger<OutboxProcessorJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = outboxRepository.GetAllUnprocessedMessages();
        Console.WriteLine(messages.Count());
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
                //Uzyj bezposrednio modulu 
                await dispatcher.DispatchAsync(
                    item,
                    context.CancellationToken);
                
                await outboxRepository.UpdateProcessedOnAsync(message.Id, DateTime.UtcNow);
                logger.LogInformation($"Processed message and updated it's state for: {message.CorrelationId}, service: {nameof(Infrastructure)}");
            } catch (Exception ex)
            {
                logger.LogError($"Job process error: {message.CorrelationId}, service: {nameof(Infrastructure)}, message: {ex.Message}");
                Console.WriteLine(ex.Message);
                // Log the exception
            }
        }
    }
}
