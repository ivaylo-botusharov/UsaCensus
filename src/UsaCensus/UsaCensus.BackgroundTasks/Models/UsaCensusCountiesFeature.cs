using System.Text.Json.Serialization;

namespace UsaCensus.BackgroundTasks.Models;

public class UsaCensusCountiesFeature
{
    [JsonPropertyName("attributes")]
    public UsaCensusCountiesFeaturesAttributes Attributes { get; set; }
}
