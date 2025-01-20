using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using UsaCensus.Infrastructure.Models;
using UsaCensus.Infrastructure.Result;

namespace UsaCensus.Infrastructure.Repositories;

public partial class DemographicsRepository : IDemographicsRepository
{
    private readonly IMongoCollection<Demographics> demographicsCollection;
    private readonly ILogger<DemographicsRepository> logger;

    private const string DatabaseErrorOccurred1001 = "Database error occurred. Internal error code: 1001.";

    private const string DatabaseErrorOccurred1002 = "Database error occurred. Internal error code: 1002.";

    private const string DatabaseErrorOccurred1003 = "Database error occurred. Internal error code: 1003.";
    
    public DemographicsRepository(
        IOptions<UsaCensusDatabaseSettings> usaCensusDatabaseSettings,
        ILogger<DemographicsRepository> logger)
    {
        MongoClient mongoClient = new (usaCensusDatabaseSettings.Value.ConnectionString);
        
        IMongoDatabase mongoDatabase = mongoClient.GetDatabase(usaCensusDatabaseSettings.Value.DatabaseName);
        
        this.demographicsCollection = mongoDatabase.GetCollection<Demographics>(
            usaCensusDatabaseSettings.Value.DemographicsCollectionName);

        this.logger = logger;
    }
    
    public async Task<Result<List<Demographics>>> GetAsync()
    {
        try
        {
            var demographics = await this.demographicsCollection.Find(_ => true).ToListAsync();
            return Result<List<Demographics>>.Success(demographics);
        }
        catch (MongoWriteException ex)
        {
            LogMongoWriteError(this.logger, ex, ex.WriteError.Code, ex.WriteError.Category.ToString());
            return Result<List<Demographics>>.Failure(DatabaseErrorOccurred1001);
        }
        catch (MongoCommandException ex)
        {
            LogMongoCommandError(this.logger, ex, ex.Code, ex.CodeName);
            return Result<List<Demographics>>.Failure(DatabaseErrorOccurred1002);
        }
        catch (MongoException ex)
        {
            LogMongoError(this.logger, ex);
            return Result<List<Demographics>>.Failure(DatabaseErrorOccurred1003);
        }
        catch (TimeoutException ex)
        {
            LogTimeoutError(this.logger, ex);
            return Result<List<Demographics>>.Failure("Timeout error occurred.");
        }
        catch (Exception ex)
        {
            LogUnexpectedError(this.logger, ex);
            return Result<List<Demographics>>.Failure("An unexpected error occurred.");
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
        catch (MongoWriteException ex)
        {
            LogMongoWriteError(this.logger, ex, ex.WriteError.Code, ex.WriteError.Category.ToString());
            return Result<Demographics?>.Failure(DatabaseErrorOccurred1001);
        }
        catch (MongoCommandException ex)
        {
            LogMongoCommandError(this.logger, ex, ex.Code, ex.CodeName);
            return Result<Demographics?>.Failure(DatabaseErrorOccurred1002);
        }
        catch (MongoException ex)
        {
            LogMongoError(this.logger, ex);
            return Result<Demographics?>.Failure(DatabaseErrorOccurred1003);
        }
        catch (TimeoutException ex)
        {
            LogTimeoutError(this.logger, ex);
            return Result<Demographics?>.Failure("Timeout error occurred.");
        }
        catch (Exception ex)
        {
            LogUnexpectedError(this.logger, ex);
            return Result<Demographics?>.Failure("An unexpected error occurred.");
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
            LogMongoWriteError(this.logger, ex, ex.WriteError.Code, ex.WriteError.Category.ToString());
            return Result<bool>.Failure(DatabaseErrorOccurred1001);
        }
        catch (MongoCommandException ex)
        {
            LogMongoCommandError(this.logger, ex, ex.Code, ex.CodeName);
            return Result<bool>.Failure(DatabaseErrorOccurred1002);
        }
        catch (MongoException ex)
        {
            LogMongoError(this.logger, ex);
            return Result<bool>.Failure(DatabaseErrorOccurred1003);
        }
        catch (TimeoutException ex)
        {
            LogTimeoutError(this.logger, ex);
            return Result<bool>.Failure("Timeout error occurred.");
        }
        catch (Exception ex)
        {
            LogUnexpectedError(this.logger, ex);
            return Result<bool>.Failure("An unexpected error occurred.");
        }
    }

    public async Task<Result<bool>> ClearCollectionAsync()
    {
        try
        {
            await this.demographicsCollection.DeleteManyAsync(FilterDefinition<Demographics>.Empty);

            return Result<bool>.Success(true);
        }
        catch (MongoWriteException ex)
        {
            LogMongoWriteError(this.logger, ex, ex.WriteError.Code, ex.WriteError.Category.ToString());
            return Result<bool>.Failure(DatabaseErrorOccurred1001);
        }
        catch (MongoCommandException ex)
        {
            LogMongoCommandError(this.logger, ex, ex.Code, ex.CodeName);
            return Result<bool>.Failure(DatabaseErrorOccurred1002);
        }
        catch (MongoException ex)
        {
            LogMongoError(this.logger, ex);
            return Result<bool>.Failure(DatabaseErrorOccurred1003);
        }
        catch (TimeoutException ex)
        {
            LogTimeoutError(this.logger, ex);
            return Result<bool>.Failure("Timeout error occurred.");
        }
        catch (Exception ex)
        {
            LogUnexpectedError(this.logger, ex);
            return Result<bool>.Failure("An unexpected error occurred.");
        }
    }
}
