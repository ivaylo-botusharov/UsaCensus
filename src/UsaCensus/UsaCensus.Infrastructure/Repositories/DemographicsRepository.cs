using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

    public async Task<Result<Demographics?>> GetByStateNameAsync(string stateName)
    {
        try
        {
            var stateDemographicsDocument = await this.demographicsCollection
                .AsQueryable()
                .FirstOrDefaultAsync(d => d.StateName.Equals(stateName, StringComparison.OrdinalIgnoreCase));
            
            if (stateDemographicsDocument == null)
            {
                return Result<Demographics?>.Success(null);
            }

            return Result<Demographics?>.Success(stateDemographicsDocument);
        }
        catch (MongoException ex)
        {
            return Result<Demographics?>.Failure($"MongoDB error occurred: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            return Result<Demographics?>.Failure($"Timeout error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<Demographics?>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task<Result<bool>> BulkInsertAsync(IList<Demographics> demographicsList)
    {
        try
        {
            await this.demographicsCollection.InsertManyAsync(demographicsList);

            return Result<bool>.Success(true);
        }
        catch (MongoWriteException ex)
        {
            return Result<bool>.Failure($"Write error occurred");
        }
        catch (MongoException ex)
        {
            return Result<bool>.Failure($"MongoDB error occurred: {ex.Message}");
        }
        catch (TimeoutException ex)
        {
            return Result<bool>.Failure($"Timeout error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }

    public async Task ClearCollectionAsync() =>
        await this.demographicsCollection.DeleteManyAsync(FilterDefinition<Demographics>.Empty);
}
