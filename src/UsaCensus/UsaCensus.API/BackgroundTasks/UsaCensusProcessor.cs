using UsaCensus.API.Models;
using Microsoft.Extensions.Options;

namespace UsaCensus.API.BackgroundTasks;

public class UsaCensusProcessor : IUsaCensusProcessor
{
    private readonly ArcGisUrlSettings arcGisUrlSettings;

    private readonly UsaCensusDatabaseSettings usaCensusDatabaseSettings;

    public UsaCensusProcessor(
        IOptions<ArcGisUrlSettings> arcGisUrlSettings,
        IOptions<UsaCensusDatabaseSettings> usaCensusDatabaseSettings)
    {
        this.arcGisUrlSettings = arcGisUrlSettings.Value;
        this.usaCensusDatabaseSettings = usaCensusDatabaseSettings.Value;
    }

    public async Task ProcessCountiesDemographicsAsync()
    {
        string arcGisBaseUrl = this.arcGisUrlSettings.BaseUrl;

        string usaCensusCountiesSegment = this.arcGisUrlSettings.UsaCensusCountiesSegment;
        
        Dictionary<string, string> usaCensusCountiesQueryParameters = new()
        {
            ["where"] = "1=1",
            ["outFields"] = "population, state_name",
            ["returnGeometry"] = "false",
            ["f"] = "pjson"
        };

        await Task.Delay(1000);
    }
}
