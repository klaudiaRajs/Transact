using Infrastructure.Interfaces;
using Transact.Core.Contracts;

namespace Infrastructure;

public class OutboxRepository(OutboxDbContext dbContext) : IOutboxRepository
{
    public async Task<bool> SaveAsync(CreateTransactionRequest item)
    {
        OutboxItem outboxItem = new OutboxItem
        {
            Id = Guid.NewGuid().ToString(),
            OccurredOn = DateTime.UtcNow,
            Type = item.MessageType,
            Payload = System.Text.Json.JsonSerializer.Serialize(item), 
            CorrelationId = item.CorrelationId
        };
        dbContext.Outbox.Add(outboxItem);
        var result = await dbContext.SaveChangesAsync(); 
        return result.Equals(1);
    }

    public List<OutboxItem> GetAllUnprocessedMessages()
    {
        return dbContext.Outbox.Where(o => o.ProcessedOn == null).ToList();
    }

    public Task UpdateProcessedOnAsync(string messageId, DateTime utcNow)
    {
        Console.WriteLine($"Updating the item state in outbox for: {nameof(messageId)} from OutboxRepository");
        var outboxItem = dbContext.Outbox.FirstOrDefault(o => o.Id == messageId);
        if (outboxItem != null)
        {
            outboxItem.ProcessedOn = utcNow;
            return dbContext.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }
}
