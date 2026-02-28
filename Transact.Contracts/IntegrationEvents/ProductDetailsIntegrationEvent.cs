using System.Text.Json;
using Transact.Core.Contracts.Infrastructure;
using Transact.Core.Contracts.Transaction;

namespace Transact.Core.Contracts.IntegrationEvents;

public class ProductDetailsIntegrationEvent : IIntegrationEvent
{
    public List<Product.Product> Products { get; set; } = new List<Product.Product>();
    public string CorrelationId { get; set; }
    public string EventType { get; set; }
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; set; }
    public string Exchange { get; set; }

    public ProductDetailsIntegrationEvent()
    {
        
    }

    public ProductDetailsIntegrationEvent(IIntegrationEvent integrationEvent)
    {
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId;
        EventType = integrationEvent.EventType;
        RoutingKey = integrationEvent.RoutingKey;
        OccurredAt = integrationEvent.OccurredAt;
        Exchange = integrationEvent.Exchange;
    }
    
    public List<int> GetProductIdsFromRequest()
    {
        var result = JsonSerializer.Deserialize<CreateTransactionRequest>(Payload);

        var ids = result.ProductIds
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        return ids; 
    }
    
    private static T DeserializeNested<T>(string json, int depth)
    {
        var current = json;

        for (int i = 0; i < depth; i++)
        {
            var wrapper = JsonSerializer.Deserialize<ProductDetailsIntegrationEvent>(current);
            current = wrapper!.Payload;
        }

        return JsonSerializer.Deserialize<T>(current)!;
    }
}
