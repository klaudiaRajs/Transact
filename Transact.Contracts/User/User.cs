using System.Text.Json.Serialization;

namespace Transact.Core.Contracts.User;

public class User
{
    [JsonPropertyName("id")]
    public string Id { get; set;  }
    [JsonPropertyName("name")]
    public string Name { get; set;  }
    [JsonPropertyName("surname")]
    public string Surname { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
