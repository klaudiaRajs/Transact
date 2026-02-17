using System.Text.Json.Serialization;

namespace Transact.Core.Contracts;

public class Transaction
{
    public string Id { get; set; }
    [JsonPropertyName("UserId")]
    public string OwnerId { get; set; }
    public string? UserSnapshot { get; set; }
    [JsonPropertyName("ProductIds")]
    public string ProductsList { get; set; }
}
