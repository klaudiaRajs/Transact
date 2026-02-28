using System.Text.Json.Serialization;
using Transact.Core.Contracts.IntegrationEvents;

namespace Transact.Core.Transactions;

public class TransactionItem
{
    [JsonPropertyName("user")]
    public IntegrationEvent User { get; set; } = default!;

    [JsonPropertyName("products")]
    public List<ProductWrapper> Products { get; set; } = new();
    
}

public class ProductWrapper
{
    [JsonPropertyName("product")]
    public IntegrationEvent Product { get; set; } = default!;
}
