using UsaCensus.API.Models;

namespace UsaCensus.API.BackgroundTasks;

public class UsaCensusProcessor : IUsaCensusProcessor
{
    private readonly ArcGisUrlSettings arcGisUrlSettings;

    private readonly UsaCensusDatabaseSettings databaseSettings;

    public UsaCensusProcessor(ArcGisUrlSettings arcGisUrlSettings, UsaCensusDatabaseSettings databaseSettings)
    {
        this.arcGisUrlSettings = arcGisUrlSettings;
        this.databaseSettings = databaseSettings;
    }

    public async Task ProcessCountiesDemographics()
    {
        await Task.Delay(1000);
    }
}
