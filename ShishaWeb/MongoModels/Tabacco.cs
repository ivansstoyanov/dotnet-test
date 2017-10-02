using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class Tabacco : BaseAuditedEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public int InternalId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;

        public double Power { get; set; } = 0;
    }
}
