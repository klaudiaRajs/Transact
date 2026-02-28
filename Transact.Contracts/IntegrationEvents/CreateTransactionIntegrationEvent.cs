using System.Text.Json;
using System.Text.Json.Serialization;
using Transact.Core.Contracts.Infrastructure;
namespace Transact.Core.Contracts.IntegrationEvents;

public class CreateTransactionIntegrationEvent : IIntegrationEvent
{
    public List<Product.Product> Products { get; set; } = new List<Product.Product>();
    public User.User User { get; set; }

    public Transactions.Transaction? Transaction { get; set; }
    public string CorrelationId { get; set; }
    public string EventType { get; set; }
    public string RoutingKey { get; set; }
    public DateTime OccurredAt { get; init; }
    public string Payload { get; set; }
    public string Exchange { get; set; }

    public CreateTransactionIntegrationEvent()
    {
    }

    public CreateTransactionIntegrationEvent(IIntegrationEvent integrationEvent)
    {
        Payload = integrationEvent.Payload;
        CorrelationId = integrationEvent.CorrelationId;
        EventType = integrationEvent.EventType;
        RoutingKey = integrationEvent.RoutingKey;
        OccurredAt = integrationEvent.OccurredAt;
        Exchange = integrationEvent.Exchange;
        Products = GetProductsFromJson(integrationEvent.Payload);
        User = GetUserFromJson(integrationEvent.Payload);
        Transaction = GetTransaction(); 
    }

    private Transactions.Transaction GetTransaction()
    {
        return new Transactions.Transaction
        {
            Id = Guid.NewGuid().ToString(), 
            OwnerId =  User.Id,
            ProductsList = JsonSerializer.Serialize(Products),
            UserSnapshot = JsonSerializer.Serialize(User), 
            CorrelationId = CorrelationId 
        };
    }
    
    private List<Product.Product> GetProductsFromJson(string json)
    {
        var root = JsonSerializer.Deserialize<RootDto>(json);
        var result = new List<Product.Product>();

        if (root?.Products == null) return result;

        foreach (var wrapper in root.Products)
        {
            var elem = wrapper.Product;

            // sprawdzamy czy produkt to tablica
            if (elem.ValueKind == JsonValueKind.Array)
            {
                var products = JsonSerializer.Deserialize<List<Product.Product>>(elem.GetRawText());
                if (products != null)
                    result.AddRange(products);
            }
        }

        return result;
    }

    private User.User? GetUserFromJson(string json)
    {
        var root = JsonSerializer.Deserialize<RootDto>(json);
        return root?.User;
    }
}

public class RootDto
{
    [JsonPropertyName("user")]
    public User.User User { get; set; }

    [JsonPropertyName("products")]
    public List<ProductWrapper> Products { get; set; }
}

public class ProductWrapper
{
    [JsonPropertyName("product")]
    public JsonElement Product { get; set; }
}
