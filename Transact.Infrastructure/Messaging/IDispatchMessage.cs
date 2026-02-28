using Transact.Core.Contracts.Infrastructure;

namespace Infrastructure.Messaging;

public interface IDispatchMessage
{
    Task Dispatch(IIntegrationEvent integrationEvent, CancellationToken ct);
}
