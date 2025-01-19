using UsaCensus.API.Repositories;
using UsaCensus.API.BackgroundTasks;

namespace UsaCensus.API.Endpoints;

public static class DemographicsEndpoints
{
    public static void MapDemographicsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder demographics = app.MapGroup("/demographics");

        demographics
            .MapGet("/", async (IDemographicsRepository demographicsRepository, IUsaCensusProcessor usaCensusProcessor) =>
            {
                var demographics = await demographicsRepository.GetAsync();

                await usaCensusProcessor.ProcessCountiesDemographicsAsync();
                
                return Results.Ok(demographics);
            })
            .WithName("GetAllDemographics");
    }
}
