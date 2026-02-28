using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class CreateTransaction(ITransactionFactory factory) : ICreateTransaction
{
    public bool Create(IIntegrationEvent integrationEvent)
    {
        var items = new CreateTransactionIntegrationEvent(integrationEvent);
        return factory.CreateTransaction(items); 
    }
}
