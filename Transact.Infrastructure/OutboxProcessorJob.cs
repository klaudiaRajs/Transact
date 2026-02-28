using Infrastructure.Interfaces;
using Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using Quartz;
using Transact.Core.Contracts.IntegrationEvents;

namespace Infrastructure;

[DisallowConcurrentExecution]
public class OutboxProcessorJob(
    IOutboxRepository outboxRepository,
    IDispatchMessage dispatcher,
    ILogger<OutboxProcessorJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await  outboxRepository.GetAllUnprocessedMessages();
        Console.WriteLine("I'm reading from outbox. I have: " + messages.Count() + " messages to process.");
        foreach (var message in messages)
        {
            //Azure + try catch - no no 
            //refactor 
            try
            {
                var integrationEvent = new IntegrationEvent()
                {
                    EventType = message.Type!,
                    OccurredAt = DateTime.UtcNow,
                    Payload = message.Payload, 
                    CorrelationId = message.CorrelationId
                };

                //Dodaj handling jak coś nie wyjdzie 
                await outboxRepository.UpdateProcessedOnAsync(message.Id, integrationEvent);
                await dispatcher.Dispatch(integrationEvent, CancellationToken.None); 

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

   
}
