using Microsoft.Extensions.Options;

using UsaCensus.API.BackgroundTasks.Models;
using UsaCensus.API.Infrastructure.Http;
using UsaCensus.API.Infrastructure.Result;
using UsaCensus.API.Models;
using UsaCensus.API.Repositories;

namespace UsaCensus.API.BackgroundTasks;

public class UsaCensusProcessor : IUsaCensusProcessor
{
    private readonly ArcGisUrlSettings arcGisUrlSettings;

    private readonly UsaCensusDatabaseSettings usaCensusDatabaseSettings;

    private readonly IHttpClientWrapper httpClientWrapper;

    private readonly IDemographicsRepository demographicsRepository;

    public UsaCensusProcessor(
        IOptions<ArcGisUrlSettings> arcGisUrlSettings,
        IOptions<UsaCensusDatabaseSettings> usaCensusDatabaseSettings,
        IHttpClientWrapper httpClientWrapper,
        IDemographicsRepository demographicsRepository)
    {
        this.arcGisUrlSettings = arcGisUrlSettings.Value;
        this.usaCensusDatabaseSettings = usaCensusDatabaseSettings.Value;
        this.httpClientWrapper = httpClientWrapper;
        this.demographicsRepository = demographicsRepository;
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

        Result<UsaCensusCounties> usaCensusCountiesResult = await this.httpClientWrapper.GetAsync<UsaCensusCounties>(
            arcGisBaseUrl,
            usaCensusCountiesSegment,
            usaCensusCountiesQueryParameters);

        if (usaCensusCountiesResult.IsFailure)
        {
            // TODO: Log error
            return;
        }

        if (usaCensusCountiesResult.Value?.Features is null || !usaCensusCountiesResult.Value.Features.Any())
        {
            return;
        }

        IEnumerable<UsaCensusCountiesFeaturesAttributes> usaCensusCountiesDemographics = usaCensusCountiesResult
            .Value
            .Features
            .Select(x => x.Attributes);
        
        IList<Demographics> usaCensusStateDemographics = usaCensusCountiesDemographics.GroupBy(x => x.StateName)
            .Select(x => new Demographics
            {
                StateName = x.Key,
                Population = x.Sum(y => y.Population ?? 0)
            })
            .ToList();

        await this.demographicsRepository.ClearCollectionAsync();

        await this.demographicsRepository.BulkInsertAsync(usaCensusStateDemographics);
    }
}
