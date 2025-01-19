using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Repositories;

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
    
    public async Task<Result<List<Demographics>>> GetAsync()
    {
        try
        {
            var demographics = await this.demographicsCollection.Find(_ => true).ToListAsync();
            return Result<List<Demographics>>.Success(demographics);
        }
        catch (MongoException ex)
        {
            return Result<List<Demographics>>.Failure($"MongoDB error occurred: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            return Result<List<Demographics>>.Failure($"Timeout error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<List<Demographics>>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Demographics?> GetByStateNameAsync(string stateName)
    {
        var filter = Builders<Demographics>.Filter.Regex(s => s.StateName, new BsonRegularExpression(stateName, "i"));
        var stateDemographicsDocument = await this.demographicsCollection.Find(filter).FirstOrDefaultAsync();
        
        return stateDemographicsDocument;
    }

    public async Task BulkInsertAsync(IList<Demographics> demographicsList) =>
        await this.demographicsCollection.InsertManyAsync(demographicsList);

    public async Task ClearCollectionAsync() =>
        await this.demographicsCollection.DeleteManyAsync(FilterDefinition<Demographics>.Empty);
}
