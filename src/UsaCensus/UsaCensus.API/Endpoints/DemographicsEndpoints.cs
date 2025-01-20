using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Repositories;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.API.Endpoints;

public static class DemographicsEndpoints
{
    public static void MapDemographicsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder demographics = app.MapGroup("/api/demographics");

        demographics
            .MapGet("/", async (IDemographicsRepository demographicsRepository) =>
            {
                Result<List<Demographics>> demographicsResult = await demographicsRepository.GetAsync();

                if (demographicsResult.IsFailure)
                {
                    return Results.InternalServerError(demographicsResult.ErrorMessage);
                }
                
                return Results.Ok(demographicsResult.Value);
            })
            .WithName("GetAllDemographics");
        
        demographics
            .MapGet("/{stateName}", async (string stateName, IDemographicsRepository demographicsRepository) =>
            {
                Result<Demographics> demographicsResult = await demographicsRepository.GetByStateNameAsync(stateName);

                if (demographicsResult.IsFailure)
                {
                    return Results.InternalServerError(demographicsResult.ErrorMessage);
                }

                if (demographicsResult.Value == null)
                {
                    return Results.NotFound($"Demographics for state '{stateName}' not found.");
                }

                return Results.Ok(demographicsResult.Value);
            })
            .WithName("GetDemographicsByStateName");
    }
}
