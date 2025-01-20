using Microsoft.Extensions.Logging;

namespace UsaCensus.Infrastructure.Http;

public partial class HttpClientWrapper
{
    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Network error occurred: {Message}")]
    public static partial void LogNetworkError(ILogger logger, string Message);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "The content type is not supported: {Message}")]
    public static partial void LogContentTypeNotSupportedError(ILogger logger, string Message);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Error deserializing the JSON response: {Message}")]
    public static partial void LogJsonDeserializationError(ILogger logger, string Message);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "An unexpected error occurred: {Message}")]
    public static partial void LogUnexpectedError(ILogger logger, string Message);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Successfully retrieved data from URL: {Url}")]
    public static partial void LogInformation(ILogger logger, string Url);
}