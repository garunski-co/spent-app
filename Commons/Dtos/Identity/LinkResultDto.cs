using System.Text.Json;
using System.Text.Json.Nodes;

namespace Spent.Commons.Dtos.Identity;

public class LinkResultDto
{
    [JsonPropertyName("publicToken")]
    public string PublicToken { get; set; }
    
    [JsonPropertyName("metadata")]
    public JsonNode Metadata { get; set; }
}
