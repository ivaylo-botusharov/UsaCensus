using UsaCensus.Infrastructure.Repositories;

namespace UsaCensus.API.Endpoints;

public static class DemographicsEndpoints
{
    public static void MapDemographicsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder demographics = app.MapGroup("/demographics");

        demographics
            .MapGet("/", async (IDemographicsRepository demographicsRepository) =>
            {
                var demographics = await demographicsRepository.GetAsync();
                
                return Results.Ok(demographics);
            })
            .WithName("GetAllDemographics");
    }
}
