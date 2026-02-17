using System.Text.Json.Serialization;
using Infrastructure.IntegrationEvents;
using Infrastructure.Messaging;
using Transact.Core.Contracts;
using Transact.Core.Transactions.Infrastructure;

namespace Transact.Core.Transactions;

public class CreateTransactionHandler(ITransactionFactory factory) : IMessageHandler<TriggerTransactionIntegrationEvent>
{
    //CreateTransaction --> Create() 
    public string UserId { get; set; }
    public string ProductIds { get; set; }
    public string CorrelationId { get; set; }
    public string MessageType { get; set; }
    public void CreateOnTransactionRequest(CreateTransactionRequest request)
    {
        ProductIds = request.ProductIds;
        //MessageType = request.MessageType;
    }
    
    public Task HandleAsync(TriggerTransactionIntegrationEvent message, CancellationToken ct)
    {
        factory.CreateTransaction(message.CreateTransactionRequest); 
        return Task.CompletedTask;
    }
}
