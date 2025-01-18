using UsaCensus.API.Services;

namespace UsaCensus.API.Endpoints;

public static class DemographicsEndpoints
{
    public static void MapDemographicsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder demographics = app.MapGroup("/demographics");

        demographics
            .MapGet("/", async (DemographicsService demographicsService) =>
            {
                var demographics = await demographicsService.GetAsync();
                
                return Results.Ok(demographics);
            })
            .WithName("GetAllDemographics");
    }
}
