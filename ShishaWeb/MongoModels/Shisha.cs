using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShishaWeb.MongoModels
{
    public class Shisha : BaseAuditedEntity
    {
        public Shisha()
        {
            this.Ratings = new ShishaRating[0];
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PublicId { get; set; }

        public string Identifier { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Picture { get; set; } = string.Empty;

        public int SmokedCount { get; set; }

        public bool IsPublic { get; set; } = false;

        public bool IsFavourite { get; set; } = false;

        public bool IsWish { get; set; } = true;

        public bool IsSeasonal { get; set; } = false;

        public double Power { get; set; } = 0;

        public string Tag { get; set; } = string.Empty;

        public Tabacco[] Tabaccos { get; set; }

        public ShishaRating[] Ratings { get; set; }

        public double TotalRating { get; set; }

        public double OwnRating { get; set; }
    }
}
