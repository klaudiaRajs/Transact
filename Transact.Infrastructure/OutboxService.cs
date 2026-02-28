using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Transaction;

namespace Infrastructure;

public class OutboxService(IOutboxRepository repository, ILogger<OutboxService> logger) : IOutboxService
{
    public async Task<bool> SaveOutboxItemAsync(CreateTransactionRequest item, string messageType)
    {
        try
        {
           var correlationId = Guid.NewGuid().ToString();
            var integrationEvent = new IntegrationEvent(correlationId)
            {
                EventType = messageType,
                OccurredAt = DateTime.UtcNow,
                Payload = System.Text.Json.JsonSerializer.Serialize(item), 
                CorrelationId = correlationId  
            };
            var result = await repository.SaveItemToOutbox(integrationEvent);
            logger.LogInformation($"Saved outbox item for: {correlationId}, service: {nameof(Infrastructure)}");
            return result.Equals(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }
    }

    public async Task<bool> SaveOutboxItemAsync(IIntegrationEvent item, string messageType)
    {
        try
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if(string.IsNullOrEmpty(item.CorrelationId))
            {
                item.CorrelationId = Guid.NewGuid().ToString();
            }
            item.EventType = messageType;
            var result = await repository.SaveItemToOutbox(item);
            logger.LogInformation($"Saved outbox item for: {item.CorrelationId}, service: {nameof(Infrastructure)}");
            return result.Equals(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }
    }
}
