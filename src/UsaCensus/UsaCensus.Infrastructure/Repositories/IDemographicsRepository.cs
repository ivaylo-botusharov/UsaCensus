using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Repositories;

public interface IDemographicsRepository
{
    Task<Result<List<Demographics>>> GetAsync();

    Task<Demographics> GetByStateNameAsync(string stateName);

    Task BulkInsertAsync(IList<Demographics> demographicsList);

    Task ClearCollectionAsync();
}
