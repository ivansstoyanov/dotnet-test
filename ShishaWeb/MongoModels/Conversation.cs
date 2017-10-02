using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class Conversation : BaseAuditedEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string[] UserIds { get; set; }
    }
}
