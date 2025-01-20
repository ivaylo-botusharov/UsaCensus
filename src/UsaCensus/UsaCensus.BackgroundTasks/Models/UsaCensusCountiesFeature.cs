using System.Text.Json.Serialization;

namespace UsaCensus.BackgroundTasks.Models;

public class UsaCensusCountiesFeature
{
    [JsonPropertyName("attributes")]
    public required UsaCensusCountiesFeaturesAttributes Attributes { get; set; }
}
