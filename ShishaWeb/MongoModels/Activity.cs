using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ShishaWeb.Config.Enumerations;

namespace ShishaWeb.MongoModels
{
    public class Activity : BaseAuditedEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public ActivityTypeEnum Type { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ItemId { get; set; }

        public ActivityComment[] Comments { get; set; }
    }
}
