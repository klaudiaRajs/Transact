using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Transact.Core.Contracts;

namespace Infrastructure;

public class OutboxService(IOutboxRepository repository, ILogger<OutboxService> logger) : IOutboxService
{
    public async Task<bool> SaveOutboxItemAsync(CreateTransactionRequest item, string messageType)
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
            item.MessageType = messageType;
            var result = await repository.SaveAsync(item);
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
