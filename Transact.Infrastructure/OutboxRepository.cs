using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure;


public class OutboxRepository(IDbContextFactory<OutboxDbContext> factory) : IOutboxRepository
{
    public async Task<bool> SaveItemToOutbox(IIntegrationEvent item)
    {
        try
        {
            await using var dbContext = await factory.CreateDbContextAsync();
            switch (item.EventType)
            {
                case ActionTypes.UserRequested:
                case ActionTypes.UserReturned:
                    var userOutboxItem = new UserOutboxItem(item);
                    dbContext.UsersOutbox.Add(userOutboxItem);
                    break;
                case ActionTypes.GetProductDetails:
                case ActionTypes.ReturnProductDetails:
                    var productOutboxItem = new ProductOutboxItem(item);
                    dbContext.ProductsOutbox.Add(productOutboxItem);
                    break;
                case ActionTypes.OrchestrateTransactionCreation:
                    var orchestratorOutboxItem = new OrchestratorOutboxItem(item);
                    dbContext.OrchestratorOutbox.Add(orchestratorOutboxItem);
                    break;
            }

            var result = await dbContext.SaveChangesAsync();
            return result.Equals(1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving product outbox item: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateProcessedOnAsync(string messageId, IIntegrationEvent item)
    {
        try
        {
            await using var dbContext = await factory.CreateDbContextAsync();
            switch (item.EventType)
            {
                case ActionTypes.UserRequested:
                case ActionTypes.UserReturned:
                    var userOutboxItem = dbContext.UsersOutbox.FirstOrDefault(a => a.Id == messageId);
                    if (userOutboxItem != null)
                    {
                        userOutboxItem.ProcessedOn = DateTime.UtcNow;
                        dbContext.UsersOutbox.Update(userOutboxItem);
                    }

                    break;
                case ActionTypes.GetProductDetails:
                case ActionTypes.ReturnProductDetails:
                    var productOutboxItem = dbContext.ProductsOutbox.FirstOrDefault(a => a.Id == messageId);
                    if (productOutboxItem != null)
                    {
                        productOutboxItem.ProcessedOn = DateTime.UtcNow;
                        dbContext.ProductsOutbox.Update(productOutboxItem);
                    }

                    break;
                case ActionTypes.OrchestrateTransactionCreation:
                    var orchestratorOutboxItem = dbContext.OrchestratorOutbox.FirstOrDefault(a => a.Id == messageId);
                    if (orchestratorOutboxItem != null)
                    {
                        orchestratorOutboxItem.ProcessedOn = DateTime.UtcNow;
                        dbContext.OrchestratorOutbox.Update(orchestratorOutboxItem);
                    }

                    break;
            }

            var result = await dbContext.SaveChangesAsync();
            return result >= 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating outbox item: {ex.Message}");
            return false;
        }
    }

    public async Task<List<OutboxItem>> GetAllUnprocessedMessages()
    {
        await using var dbContext = await factory.CreateDbContextAsync();
        var result = new List<OutboxItem>();
        result.AddRange(dbContext.ProductsOutbox.Where(o => o.ProcessedOn == null).Select(a => new OutboxItem
        {
            Id = a.Id,
            CorrelationId = a.CorrelationId,
            OccurredOn = a.OccurredOn,
            Payload = a.Payload,
            Type = a.Type
        }).ToList());
        result.AddRange(dbContext.UsersOutbox.Where(o => o.ProcessedOn == null).Select(a => new OutboxItem
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
        result.AddRange(dbContext.TransactionDataOutbox.Select(a => new TransactionDataOutboxItem
        {
            Id = a.Id,
            CorrelationId = a.CorrelationId,
            Payload = a.Payload,
            OccurredOn = a.OccurredOn,
            Type = a.Type
        }).ToList());
        return result;
    }
}
