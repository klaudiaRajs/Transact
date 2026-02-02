using System.Text.Json.Serialization;

namespace Transact.Core.Contracts;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set;  }
    [JsonPropertyName("name")]
    public string Name { get; set;  }
    [JsonPropertyName("surname")]
    public string Surname { get; set; }
    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
