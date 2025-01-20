using MongoDB.Bson;
using MongoDB.Driver;

using UsaCensus.Infrastructure.Database.Models;

namespace UsaCensus.Infrastructure.Database.Initializers;

public class DatabaseInitializer
{
    private readonly IMongoClient mongoClient;
    private readonly string databaseName;
    private readonly string collectionName;
    
    public DatabaseInitializer(UsaCensusDatabaseSettings? settings)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        this.mongoClient = new MongoClient(settings.ConnectionString);
        this.databaseName = settings.DatabaseName;
        this.collectionName = settings.DemographicsCollectionName;
    }

    public void Initialize()
    {
        var database = this.mongoClient.GetDatabase(this.databaseName);
        var collectionExists = CollectionExists(database, this.collectionName);

        if (!collectionExists)
        {
            database.CreateCollection(this.collectionName);
        }
    }

    private static bool CollectionExists(IMongoDatabase database, string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });

        return collections.Any();
    }
}
