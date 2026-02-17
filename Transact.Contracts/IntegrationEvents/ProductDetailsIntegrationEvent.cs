using Transact.Core.Contracts;

namespace Infrastructure.IntegrationEvents;

public class ProductDetailsIntegrationEvent(string correlationId) : IntegrationEvent(correlationId)
{
    public List<Product> Products { get; set; } = new List<Product>();
}
