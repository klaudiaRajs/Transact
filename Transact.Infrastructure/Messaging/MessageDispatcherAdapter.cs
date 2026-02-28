using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure.Messaging;

public class MessageDispatcherAdapter(IServiceProvider serviceProvider) : IDispatchMessage
{
    public async Task Dispatch(IIntegrationEvent integrationEvent, CancellationToken ct)
    {
        var messageType = integrationEvent.EventType.GetType();
        var responseMessage = System.Text.Json.JsonSerializer.Deserialize(integrationEvent.Payload, messageType);
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);

        var handler = serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException(
                $"No handler registered for {messageType.Name}");
        }

        var method = handlerType.GetMethod("HandleAsync")!;
        await ((Task?)method.Invoke(handler, new[] { responseMessage, ct }) ?? throw new InvalidOperationException("HandleAsync returned null"));
    }
}
