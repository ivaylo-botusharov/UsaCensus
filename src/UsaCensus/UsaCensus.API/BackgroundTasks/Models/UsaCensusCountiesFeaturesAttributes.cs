using System.Text.Json.Serialization;

namespace UsaCensus.API.BackgroundTasks.Models;

public class UsaCensusCountiesFeaturesAttributes
{
    [JsonPropertyName("POPULATION")]
    public long? Population { get; set; }

    [JsonPropertyName("STATE_NAME")]
    public string StateName { get; set; }
}
