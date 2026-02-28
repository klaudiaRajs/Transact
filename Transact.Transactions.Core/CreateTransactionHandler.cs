using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class CreateTransactionHandler(ITransactionFactory factory) : IMessageHandler<CreateTransactionIntegrationEvent>
{
    //CreateTransaction --> Create() 
    public string UserId { get; set; }
    public string ProductIds { get; set; }
    public string CorrelationId { get; set; }
    public string MessageType { get; set; }
    
    public Task HandleAsync(CreateTransactionIntegrationEvent message, CancellationToken ct)
    {
        factory.CreateTransaction(message); 
        return Task.CompletedTask;
    }
}
