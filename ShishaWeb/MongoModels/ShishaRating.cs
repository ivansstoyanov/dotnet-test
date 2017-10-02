using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShishaWeb.MongoModels
{
    public class ShishaRating : BaseAuditedEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        public double Value { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ShishaId { get; set; } = string.Empty;
    }
}
