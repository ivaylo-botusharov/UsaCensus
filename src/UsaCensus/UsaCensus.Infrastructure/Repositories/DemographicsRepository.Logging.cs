using Microsoft.Extensions.Logging;

namespace UsaCensus.Infrastructure.Repositories;

public partial class DemographicsRepository
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "MongoDB write error occurred while getting demographics. Code: {Code}, CodeName: {CodeName}")]
    public static partial void LogMongoWriteError(ILogger logger, Exception ex, int Code, string CodeName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "MongoDB command error occurred while getting demographics. Code: {Code}, CodeName: {CodeName}")]
    public static partial void LogMongoCommandError(ILogger logger, Exception ex, int Code, string CodeName);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "MongoDB error occurred while getting demographics.")]
    public static partial void LogMongoError(ILogger logger, Exception ex);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "Timeout error occurred.")]
    public static partial void LogTimeoutError(ILogger logger, Exception ex);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "An unexpected error occurred.")]
    public static partial void LogUnexpectedError(ILogger logger, Exception ex);
}