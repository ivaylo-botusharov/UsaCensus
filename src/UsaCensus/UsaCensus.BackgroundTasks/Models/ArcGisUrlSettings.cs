namespace UsaCensus.BackgroundTasks.Models;

public class ArcGisUrlSettings
{
    public required string BaseUrl { get; set; }

    public required string UsaCensusCountiesSegment { get; set; }

    public static string SectionName => "ArcGisUrlSettings";
}
