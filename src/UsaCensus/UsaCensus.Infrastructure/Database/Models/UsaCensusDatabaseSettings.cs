namespace UsaCensus.Infrastructure.Database.Models;

public class UsaCensusDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string DemographicsCollectionName { get; set; } = null!;

    public static string SectionName => "UsaCensusDatabase";
}
