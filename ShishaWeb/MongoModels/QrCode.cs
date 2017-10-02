using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class QrCode : BaseAuditedEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public string Value { get; set; }

        public int Count { get; set; }
    }
}
