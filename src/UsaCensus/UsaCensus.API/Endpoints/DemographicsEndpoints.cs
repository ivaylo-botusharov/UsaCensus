using UsaCensus.Infrastructure.Repositories;

namespace UsaCensus.API.Endpoints;

public static class DemographicsEndpoints
{
    public static void MapDemographicsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder demographics = app.MapGroup("/api/demographics");

        demographics
            .MapGet("/", async (IDemographicsRepository demographicsRepository) =>
            {
                var demographics = await demographicsRepository.GetAsync();
                
                return Results.Ok(demographics);
            })
            .WithName("GetAllDemographics");
        
        demographics
            .MapGet("/{stateName}", async (string stateName, IDemographicsRepository demographicsRepository) =>
            {
                var demographic = await demographicsRepository.GetByStateNameAsync(stateName);
                
                if (demographic == null)
                {
                    return Results.NotFound($"Demographics for state '{stateName}' not found.");
                }

                return Results.Ok(demographic);
            })
            .WithName("GetDemographicsByStateName");
    }
}
