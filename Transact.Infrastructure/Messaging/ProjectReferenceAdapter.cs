using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Products;
using Transact.Core.Transactions.Infrastructure;
using Transact.Core.Users;
using Transact.Orchestrator;

namespace Infrastructure.Messaging;

public class ProjectReferenceAdapter(ICreateTransaction createTransaction, OrchestrateTransaction orchestrateTransaction, GetProductDetails productDetails, GetOrCreateUserDetails getOrCreateUserDetails) : IDispatchMessage
{
    public async Task Dispatch(IIntegrationEvent integrationEvent, CancellationToken ct)
    {
        switch (integrationEvent.EventType)
        {
            case ActionTypes.UserRequested:
                await getOrCreateUserDetails.Get(integrationEvent);
                break; 
            case ActionTypes.GetProductDetails:
                await productDetails.Get(integrationEvent);
                break; 
            case ActionTypes.OrchestrateTransactionCreation:
                await orchestrateTransaction.Orchestrate(integrationEvent);
                break; 
            case ActionTypes.CreateTransactionRequest:
                createTransaction.Create(integrationEvent);
                break; 
            default:
                throw new InvalidOperationException($"No handler for event type {integrationEvent.EventType}");
        }
    }
}
