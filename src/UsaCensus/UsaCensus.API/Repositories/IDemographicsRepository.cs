using UsaCensus.API.Models;

namespace UsaCensus.API.Repositories;

public interface IDemographicsRepository
{
    Task<List<Demographics>> GetAsync();
    
    Task<Demographics?> GetAsync(string id);
    
    Task CreateAsync(Demographics newDemographics);
    
    Task UpdateAsync(string id, Demographics updatedDemographics);
    
    Task RemoveAsync(string id);

    Task BulkInsertAsync(IList<Demographics> demographicsList);

    Task ClearCollectionAsync();
}
