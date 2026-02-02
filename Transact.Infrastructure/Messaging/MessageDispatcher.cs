namespace Infrastructure.Messaging;

public class MessageDispatcher(IServiceProvider serviceProvider) : IMessageDispatcher
{
    //Dispatch() 
    //Event jako past not about to 
    public async Task DispatchAsync(object message, CancellationToken ct)
    {
        var messageType = message.GetType();
        var handlerType = typeof(IMessageHandler<>).MakeGenericType(messageType);

        var handler = serviceProvider.GetService(handlerType);
        if (handler == null)
        {
            throw new InvalidOperationException(
                $"No handler registered for {messageType.Name}");
        }

        var method = handlerType.GetMethod("HandleAsync")!;
        await ((Task?)method.Invoke(handler, new[] { message, ct }) ?? throw new InvalidOperationException("HandleAsync returned null"));
    }
}
