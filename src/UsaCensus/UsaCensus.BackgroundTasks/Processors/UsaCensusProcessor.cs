using Microsoft.Extensions.Options;

using UsaCensus.BackgroundTasks.Models;
using UsaCensus.Infrastructure.Http;
using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Repositories;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.BackgroundTasks.Processors;

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
        Result<UsaCensusCounties> usaCensusCountiesResult = await this.GetUsaCensusCountiesResultAsync();

        if (usaCensusCountiesResult.IsFailure)
        {
            // TODO: Log error
            return;
        }

        if (usaCensusCountiesResult.Value?.Features is null || !usaCensusCountiesResult.Value.Features.Any())
        {
            // TODO: Log message that there are no features
            return;
        }

        IList<Demographics> usaCensusStateDemographics = this.CalculateStatePopulationTotals(usaCensusCountiesResult);

        await this.demographicsRepository.ClearCollectionAsync();

        await this.demographicsRepository.BulkInsertAsync(usaCensusStateDemographics);
    }

    private async Task<Result<UsaCensusCounties>> GetUsaCensusCountiesResultAsync()
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

        return usaCensusCountiesResult;
    }

    // aggregates population by state
    private IList<Demographics> CalculateStatePopulationTotals(Result<UsaCensusCounties> usaCensusCountiesResult)
    {
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
        
        return usaCensusStateDemographics;
    }
}
