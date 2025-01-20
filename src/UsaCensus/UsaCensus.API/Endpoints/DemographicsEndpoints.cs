using UsaCensus.API.Models;
using UsaCensus.Infrastructure.Database.Models;
using UsaCensus.Infrastructure.Database.Repositories;
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

                List<DemographicsResponse> demographics = demographicsResult
                    .Value?
                    .Select(demographics => DemographicsResponse.Create(demographics))
                    .ToList() ?? new List<DemographicsResponse>();
                
                return Results.Ok(demographics);
            })
            .WithName("GetAllDemographics");
        
        demographics
            .MapGet("/{stateName}", async (string stateName, IDemographicsRepository demographicsRepository) =>
            {
                Result<Demographics?> demographicsResult = await demographicsRepository.GetByStateNameAsync(stateName);

                if (demographicsResult.IsFailure)
                {
                    return Results.InternalServerError(demographicsResult.ErrorMessage);
                }

                if (demographicsResult.Value == null)
                {
                    return Results.NotFound($"Demographics for state '{stateName}' not found.");
                }

                DemographicsResponse demographicsForState = DemographicsResponse.Create(demographicsResult.Value);

                return Results.Ok(demographicsForState);
            })
            .WithName("GetDemographicsByStateName");
    }
}
