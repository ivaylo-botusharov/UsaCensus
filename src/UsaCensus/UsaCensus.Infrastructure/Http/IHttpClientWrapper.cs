using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Http;

public interface IHttpClientWrapper
{
    Task<Result<T>> GetAsync<T>(
        string baseUrl,
        string segment,
        Dictionary<string, string>? parameters = null);
}
