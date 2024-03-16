using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace task_api.Models;

public class Work
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string title { get; set; } = null!;
    
    public string text { get; set; } = null!;
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("user")]
    public string? user { get; set; }
    public int __v { get; set; } = 0!;
}
