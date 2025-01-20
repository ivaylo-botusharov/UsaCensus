using UsaCensus.Infrastructure.Database.Models;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Database.Repositories;

public interface IDemographicsRepository
{
    Task<Result<List<Demographics>>> GetAsync();

    Task<Result<Demographics?>> GetByStateNameAsync(string stateName);

    Task<Result<bool>> BulkInsertAsync(IList<Demographics> demographicsList);

    Task<Result<bool>> ClearCollectionAsync();
}
