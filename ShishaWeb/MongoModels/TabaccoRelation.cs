using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class TabaccoRelation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public Tabacco[] RelatedTabaccos { get; set; }
    }
}
