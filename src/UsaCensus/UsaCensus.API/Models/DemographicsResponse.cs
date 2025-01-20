using UsaCensus.Infrastructure.Database.Models;

namespace UsaCensus.API.Models;

public class DemographicsResponse
{
    public long? Population { get; set; }

    public required string StateName { get; set; }

    public static DemographicsResponse Create(Demographics demographics) =>
        new DemographicsResponse
        {
            Population = demographics.Population,
            StateName = demographics.StateName
        };
}
