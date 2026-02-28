using Transact.Core.Contracts.Infrastructure;

namespace Transact.Core.Transactions.Infrastructure;

public interface ICreateTransaction 
{
    public bool Create(IIntegrationEvent integrationEvent); 
}
