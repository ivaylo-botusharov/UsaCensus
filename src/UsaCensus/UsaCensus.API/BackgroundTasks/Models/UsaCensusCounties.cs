using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace UsaCensus.API.BackgroundTasks.Models;

public class UsaCensusCounties
{
    [JsonPropertyName("features")]
    public IList<UsaCensusCountiesFeature> Features { get; set; }
}
