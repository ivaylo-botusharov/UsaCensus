using System.Text.Json.Serialization;

namespace UsaCensus.API.BackgroundTasks.Models;

public class UsaCensusCountiesFeature
{
    [JsonPropertyName("attributes")]
    public UsaCensusCountiesFeaturesAttributes Attributes { get; set; }
}
