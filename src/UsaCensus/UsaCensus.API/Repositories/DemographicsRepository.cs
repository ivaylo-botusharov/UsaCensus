using Microsoft.Extensions.Options;
using MongoDB.Driver;

using UsaCensus.API.Models;

namespace UsaCensus.API.Repositories;

public class DemographicsRepository : IDemographicsRepository
{
    private readonly IMongoCollection<Demographics> demographicsCollection;
    
    public DemographicsRepository(IOptions<UsaCensusDatabaseSettings> usaCensusDatabaseSettings)
    {
        MongoClient mongoClient = new (usaCensusDatabaseSettings.Value.ConnectionString);
        
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(usaCensusDatabaseSettings.Value.DatabaseName);
        
        this.demographicsCollection = mongoDatabase.GetCollection<Demographics>(
            usaCensusDatabaseSettings.Value.DemographicsCollectionName);
    }
    
    public async Task<List<Demographics>> GetAsync() =>
        await this.demographicsCollection.Find(_ => true).ToListAsync();
    
    public async Task<Demographics?> GetAsync(string id) =>
        await this.demographicsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task CreateAsync(Demographics newDemographics) =>
        await this.demographicsCollection.InsertOneAsync(newDemographics);
    
    public async Task UpdateAsync(string id, Demographics updatedDemographics) =>
        await this.demographicsCollection.ReplaceOneAsync(x => x.Id == id, updatedDemographics);
    
    public async Task RemoveAsync(string id) =>
        await this.demographicsCollection.DeleteOneAsync(x => x.Id == id);

    // public async Task BulkInsertAsync(List<Demographics> stateDemographics)
    // {
    //     var stateDemographicsBsonDocuments = stateDemographics.Select(sd => new BsonDocument
    //     {
    //         { "population", sd.Population },
    //         { "stateName", sd.StateName }
    //     }).ToList();

    //     await this.demographicsCollection.InsertManyAsync(stateDemographicsBsonDocuments);
    // }

    public async Task BulkInsertAsync(IList<Demographics> demographicsList) =>
        await this.demographicsCollection.InsertManyAsync(demographicsList);

    public async Task ClearCollectionAsync() =>
        await this.demographicsCollection.DeleteManyAsync(FilterDefinition<Demographics>.Empty);
}
