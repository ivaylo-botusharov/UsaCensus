using Microsoft.Extensions.Logging;

namespace UsaCensus.BackgroundTasks.Processors;

public partial class UsaCensusProcessor
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Error while fetching USA Census Counties.")]
    public static partial void LogFetchingCensusCountiesError(ILogger logger);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Failed to clear demographics collection.")]
    public static partial void LogClearCollectionError(ILogger logger);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "No features found in USA Census Counties.")]
    public static partial void LogNoFeaturesFound(ILogger logger);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Error during bulk insert of demographics.")]
    public static partial void LogBulkInsertError(ILogger logger);
}