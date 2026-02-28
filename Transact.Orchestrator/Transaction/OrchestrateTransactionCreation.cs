using System.Text.Json;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transaction;

namespace Transact.Orchestrator.Transaction;

public class OrchestrateTransactionCreation(OrchestrateRepository repository)
{
    public async Task OrchestrateCreation(string actionType, CreateTransactionRequest payload)
    {
        if (actionType == ActionTypes.CreateTransactionRequest)
        {
            var transaction = new OrchestratorTransaction
            {
                CorrelationId = Guid.NewGuid().ToString(),
                Id = Guid.NewGuid().ToString(),
                EventToRaise = "TransactionCreationRequested",
                Payload = JsonSerializer.Serialize(payload)
            };
            await repository.SaveOrchestratorTransaction(transaction);
        }
    }
}
