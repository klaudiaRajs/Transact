using Transact.Core.Contracts;

namespace Infrastructure.IntegrationEvents;

public class CreateTransactionIntegrationEvent(string CorrelationId) : IntegrationEvent(CorrelationId)
{
    public List<Product> Products { get; set; } = new List<Product>();
    public User User { get; set; }
}
