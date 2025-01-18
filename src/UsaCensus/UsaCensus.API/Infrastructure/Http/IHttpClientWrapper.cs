using UsaCensus.API.Infrastructure.Result;

namespace UsaCensus.API.Infrastructure.Http;

public interface IHttpClientWrapper
{
    Task<Result<T>> GetAsync<T>(
        string baseUrl,
        string segment,
        Dictionary<string, string>? parameters = null);
}
