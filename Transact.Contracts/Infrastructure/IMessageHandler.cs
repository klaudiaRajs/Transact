namespace Transact.Core.Contracts.Infrastructure;

public interface IMessageHandler<in TMessage>
{
    Task HandleAsync(TMessage message, CancellationToken ct);
}
