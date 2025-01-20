using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsaCensus.Infrastructure.Database.Models;

public class Demographics
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("population")]
    public long Population { get; set; }

    [BsonElement("stateName")]
    public string StateName { get; set; }
}
