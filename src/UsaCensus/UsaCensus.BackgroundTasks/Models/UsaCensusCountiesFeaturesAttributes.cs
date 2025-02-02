using System.Text.Json.Serialization;

namespace UsaCensus.BackgroundTasks.Models;

public class UsaCensusCountiesFeaturesAttributes
{
    [JsonPropertyName("POPULATION")]
    public long? Population { get; set; }

    [JsonPropertyName("STATE_NAME")]
    public required string StateName { get; set; }
}
