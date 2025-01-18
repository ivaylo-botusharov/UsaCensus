using System.Text.Json;
using UsaCensus.API.Infrastructure.Result;

namespace UsaCensus.API.Infrastructure.Http;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<HttpClientWrapper> logger;

    public HttpClientWrapper(IHttpClientFactory httpClientFactory, ILogger<HttpClientWrapper> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    public async Task<Result<T>> GetAsync<T>(
        string baseUrl,
        string segment,
        Dictionary<string, string>? parameters = null)
    {
        string url = BuildUrl(baseUrl, segment, parameters);

        using HttpClient httpClient = httpClientFactory.CreateClient();
        
        try
        {
            T? result = await httpClient.GetFromJsonAsync<T>(
                url,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return Result<T>.Success(result);
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogError($"Network error: {ex.Message}");
            return Result<T>.Failure("A network error occurred. Please, contact administrator (error code: 9110)");
        }
        catch (NotSupportedException ex)
        {
            this.logger.LogError($"The content type is not supported: {ex.Message}");
            return Result<T>.Failure("The content type is not supported. Please, contact administrator (error code: 9215)");
        }
        catch (JsonException ex)
        {
            this.logger.LogError($"Error deserializing the JSON response: {ex.Message}");
            return Result<T>.Failure("An error occurred. Please, contact administrator (error code: 9200)");
        }
        catch (Exception ex)
        {
            this.logger.LogError($"An unexpected error occurred: {ex.Message}");
            return Result<T>.Failure("An unexpected error occurred. Please, contact administrator (error code: 9000)");
        }
    }

    private static string BuildUrl(string baseUrl, string segment, Dictionary<string, string>? parameters)
    {
        if (parameters != null && parameters.Any())
        {
            string queryString = string.Join(
                "&",
                parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));

            return $"{baseUrl}/{segment}?{queryString}";
        }

        return $"{baseUrl}/{segment}";
    }
}
