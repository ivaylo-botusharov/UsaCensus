using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Http;

public partial class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory httpClientFactory;
    
    private readonly ILogger<HttpClientWrapper> logger;

    private const string NetworkErrorMessage = "A network error occurred. Please, contact administrator (error code: 9110)";

    private const string ContentTypeNotSupportedErrorMessage = "The content type is not supported. Please, contact administrator (error code: 9215)";

    private const string JsonDeserializationErrorMessage = "An error occurred. Please, contact administrator (error code: 9200)";

    private const string UnexpectedErrorMessage = "An unexpected error occurred. Please, contact administrator (error code: 9000)";

    public HttpClientWrapper(IHttpClientFactory httpClientFactory, ILogger<HttpClientWrapper> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public async Task<Result<T>> GetAsync<T>(
        string baseUrl,
        string segment,
        Dictionary<string, string>? queryParameters = null)
    {
        string url = BuildUrl(baseUrl, segment, queryParameters);

        HttpClient httpClient = httpClientFactory.CreateClient();
        
        try
        {
            T? result = await httpClient.GetFromJsonAsync<T>(
                url,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            LogInformation(this.logger, url);
            return Result<T>.Success(result);
        }
        catch (HttpRequestException ex)
        {
            LogNetworkError(this.logger, ex.Message);
            return Result<T>.Failure(NetworkErrorMessage);
        }
        catch (NotSupportedException ex)
        {
            LogContentTypeNotSupportedError(this.logger, ex.Message);
            return Result<T>.Failure(ContentTypeNotSupportedErrorMessage);
        }
        catch (JsonException ex)
        {
            LogJsonDeserializationError(this.logger, ex.Message);
            return Result<T>.Failure(JsonDeserializationErrorMessage);
        }
        catch (Exception ex)
        {
            LogUnexpectedError(this.logger, ex.Message);
            return Result<T>.Failure(UnexpectedErrorMessage);
        }
    }

    private static string BuildUrl(string baseUrl, string segment, Dictionary<string, string>? queryParameters)
    {
        if (queryParameters != null && queryParameters.Any())
        {
            string queryString = string.Join(
                "&",
                queryParameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            return $"{baseUrl}/{segment}?{queryString}";
        }

        return $"{baseUrl}/{segment}";
    }
}
