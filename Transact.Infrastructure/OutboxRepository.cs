using Infrastructure.IntegrationEvents;
using Infrastructure.Interfaces;
using Transact.Core.Contracts;

namespace Infrastructure;

public class OutboxRepository(OutboxDbContext dbContext) : IOutboxRepository
{
    public async Task<bool> SaveTransactionOutboxItem(CreateTransactionRequest item)
    {
        TransactionOutboxItem outboxItem = new TransactionOutboxItem()
        {
            Id = Guid.NewGuid().ToString(),
            OccurredOn = DateTime.UtcNow,
            //Type = item.MessageType,
            Payload = System.Text.Json.JsonSerializer.Serialize(item), 
        };
        dbContext.TransactionsOutbox.Add(outboxItem);
        var result = await dbContext.SaveChangesAsync(); 
        return result.Equals(1);
    }
    
    public async Task<bool> SaveProductOutboxItem(IntegrationEvent item)
    {
        try
        {
            ProductOutboxItem outboxItem = new ProductOutboxItem()
            {
                Id = Guid.NewGuid().ToString(),
                OccurredOn = DateTime.UtcNow,
                Type = "ReturnProductDetails",
                Payload = System.Text.Json.JsonSerializer.Serialize(item), 
                CorrelationId = item.CorrelationId
            };
            dbContext.ProductsOutbox.Add(outboxItem);
            var result = await dbContext.SaveChangesAsync(); 
            return result.Equals(1);
        } catch (Exception ex)
        {
            Console.WriteLine($"Error saving product outbox item: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> SaveOrchestratorOutboxItem(IntegrationEvent item)
    {
        try
        {
            OrchestratorOutboxItem outboxItem = new OrchestratorOutboxItem()
            {
                Id = Guid.NewGuid().ToString(),
                OccurredOn = DateTime.UtcNow,
                Type = item.EventType,
                Payload = System.Text.Json.JsonSerializer.Serialize(item), 
                CorrelationId = item.CorrelationId
            };
            dbContext.OrchestratorOutbox.Add(outboxItem);
            var result = await dbContext.SaveChangesAsync(); 
            return result.Equals(1);
        } catch (Exception ex)
        {
            Console.WriteLine($"Error saving product outbox item: {ex.Message}");
            return false;
        }
    }

    public List<OutboxItem> GetAllUnprocessedMessages()
    {
        var result = new List<OutboxItem>(); 
        result.AddRange(dbContext.TransactionsOutbox.Where(o => o.ProcessedOn == null).Select(a => new OutboxItem
        {
            Id = a.Id,
            CorrelationId = a.CorrelationId,
            OccurredOn = a.OccurredOn,
            Payload = a.Payload,
            Type = a.Type
        }).ToList());
        result.AddRange(dbContext.ProductsOutbox.Where(o => o.ProcessedOn == null).Select(a => new OutboxItem
        {
            Id = a.Id,
            CorrelationId = a.CorrelationId,
            OccurredOn = a.OccurredOn,
            Payload = a.Payload,
            Type = a.Type
        }).ToList());
        result.AddRange(dbContext.OrchestratorOutbox.Where(o => o.ProcessedOn == null).Select(a => new OutboxItem
        {
            Id = a.Id,
            CorrelationId = a.CorrelationId,
            OccurredOn = a.OccurredOn,
            Payload = a.Payload,
            Type = a.Type
        }).ToList());
        return result;
    }

    public Task UpdateProcessedOnAsync(string messageId, DateTime utcNow)
    {
        Console.WriteLine($"Updating the item state in outbox for: {messageId} from OutboxRepository");
        var outboxItem = dbContext.TransactionsOutbox.FirstOrDefault(o => o.Id == messageId);
        if (outboxItem != null)
        {
            outboxItem.ProcessedOn = utcNow;
            return dbContext.SaveChangesAsync();
        }
        return Task.CompletedTask;
    }
}
