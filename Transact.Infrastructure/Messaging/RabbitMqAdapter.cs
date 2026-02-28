using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.IntegrationEvents;
using Transact.Core.Contracts.Product;
using Transact.Core.Contracts.Transaction;
using Transact.Core.Contracts.User;

namespace Infrastructure.Messaging;

public class RabbitMqAdapter(RabbitMqEventBus dispatcher) : IDispatchMessage
{
    public async Task Dispatch(IIntegrationEvent integrationEvent, CancellationToken ct)
    {
        integrationEvent = GetRoutingKeyAndExchange(integrationEvent as IntegrationEvent, integrationEvent.EventType);
        await dispatcher.PublishAsync(
            integrationEvent,
            ct);
    }
    
    private static IntegrationEvent GetRoutingKeyAndExchange(IntegrationEvent integrationEvent, string messageType)
    {
        var routingKey = "";
        var exchange = "";
        switch (messageType)
        {
            case ActionTypes.CreateTransactionRequest:
                routingKey = TransactionMessaging.RoutingKey;
                exchange = TransactionMessaging.Exchange;
                break;
            case ActionTypes.OrchestrateTransactionCreation:
            case ActionTypes.ReturnProductDetails:
                routingKey = OrchestratorMessaging.RoutingKey;
                exchange = OrchestratorMessaging.Exchange;
                break;
            case ActionTypes.GetProductDetails:
                routingKey = ProductMessaging.RoutingKey;
                exchange = ProductMessaging.Exchange;
                break;
            case ActionTypes.UserRequested:
            case ActionTypes.UserReturned:
                routingKey = UserMessaging.RoutingKey;
                exchange = UserMessaging.Exchange;
                break;
        }

        integrationEvent.RoutingKey = routingKey;
        integrationEvent.Exchange = exchange;
        return integrationEvent;
    }
}
