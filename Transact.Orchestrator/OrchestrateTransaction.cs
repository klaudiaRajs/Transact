using Infrastructure.Interfaces;
using Transact.Core.Contracts.Infrastructure;
using Transact.Orchestrator.Transaction;

namespace Transact.Orchestrator;

public class OrchestrateTransaction (OrchestrateRepository repository, IOutboxService outboxService)
{
    public async Task<bool> Orchestrate(IIntegrationEvent integrationEvent)
    {
        try
        {
            
            var orchestrateTransactionItem = new OrchestratorTransaction
            {
                CorrelationId = integrationEvent.CorrelationId,
                CreatedAt = DateTime.UtcNow,
                Status = "Triggered",
                EventToRaise = ActionTypes.CreateTransactionRequest,
                Payload = integrationEvent.Payload
            };
            await repository.SaveOrchestratorTransaction(orchestrateTransactionItem);
            //TODO To duplikuje moje event
            await outboxService.SaveOutboxItemAsync(integrationEvent, ActionTypes.GetProductDetails);
            await outboxService.SaveOutboxItemAsync(integrationEvent, ActionTypes.UserRequested); 
            return true; 
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false; 
        }
    }
}
