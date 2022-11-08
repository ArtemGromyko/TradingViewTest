using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TradingViewTest.Models;

public class EntityBase
{
    [JsonIgnore]
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string EntityId { get; set; }
}
