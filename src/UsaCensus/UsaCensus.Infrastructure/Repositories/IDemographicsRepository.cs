using UsaCensus.Infrastructure.Models;

namespace UsaCensus.Infrastructure.Repositories;

public interface IDemographicsRepository
{
    Task<List<Demographics>> GetAsync();
    
    Task<Demographics?> GetAsync(string id);
    
    Task CreateAsync(Demographics newDemographics);
    
    Task UpdateAsync(string id, Demographics updatedDemographics);
    
    Task RemoveAsync(string id);

    Task BulkInsertAsync(IList<Demographics> demographicsList);

    Task ClearCollectionAsync();

    Task<Demographics> GetByStateNameAsync(string stateName);
}
