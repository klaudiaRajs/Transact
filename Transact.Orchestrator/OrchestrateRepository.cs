using Transact.Orchestrator.Transaction;

namespace Transact.Orchestrator;

public class OrchestrateRepository(OrchestratorDbContext context)
{
    public async Task<Task> SaveOrchestratorTransaction(OrchestratorTransaction transaction)
    {
        try
        {
            await context.OrchestratorTransactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            return Task.CompletedTask;
        }
    }

    public Task<OrchestratorTransaction?> GetStatusByCorrelationId(string correlationId)
    {
        return Task.FromResult(context.OrchestratorTransactions.FirstOrDefault(tr => tr.CorrelationId == correlationId));
    }
    
    public Task UpdateTransactionStatus(string correlationId, string status, string payload)
    {
        var transaction = context.OrchestratorTransactions.FirstOrDefault(tr => tr.CorrelationId == correlationId);
        if (transaction != null)
        {
            transaction.Status = status;
            transaction.ProcessedAt = DateTime.UtcNow;
            transaction.Payload = payload;
            context.SaveChanges();
        }
        return Task.CompletedTask;
    }
}
