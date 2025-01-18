using MongoDB.Bson;
using MongoDB.Driver;
using UsaCensus.API.Models;

namespace UsaCensus.API.Initializers;

public class DatabaseInitializer
{
    private readonly IMongoClient mongoClient;
    private readonly string databaseName;
    private readonly string collectionName;

    private readonly List<BsonDocument> initialDocuments =
    [
        new BsonDocument { { "population", 123 }, { "stateName", "Arizona" } },
        new BsonDocument { { "population", 345 }, { "stateName", "California" } }
    ];

    public DatabaseInitializer(UsaCensusDatabaseSettings settings)
    {
        this.mongoClient = new MongoClient(settings.ConnectionString);
        this.databaseName = settings.DatabaseName;
        this.collectionName = settings.DemographicsCollectionName;
    }

    public void Initialize()
    {
        var database = this.mongoClient.GetDatabase(this.databaseName);
        var collection = database.GetCollection<BsonDocument>(this.collectionName);

        var filter = new BsonDocument("name", this.collectionName);
        var collections = database.ListCollections(new ListCollectionsOptions { Filter = filter });

        if (!collections.Any())
        {
            database.CreateCollection(this.collectionName);

            collection.InsertMany(initialDocuments);
        }
        else
        {
            var documentCount = collection.CountDocuments(new BsonDocument());
            
            if (documentCount == 0)
            {
                collection.InsertMany(initialDocuments);
            }
        }
    }
}
